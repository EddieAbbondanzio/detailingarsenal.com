using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Persistence.Shared;
using DetailingArsenal.Domain;
using DetailingArsenal.Shared;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesRepo : DatabaseInteractor, IPadSeriesRepo {
        public PadSeriesRepo(IDatabase database) : base(database) { }

        public async Task<PadSeries?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                        @"  select * from pad_series where id = @Id;
                            select * from pad_series_polisher_types where pad_series_id = @Id;
                            select * from pad_sizes where pad_series_id = @Id;
                            select pi.*, i.* from images i join pad_images pi on i.id = pi.image_id join pads p on pi.pad_id = p.id where p.pad_series_id = @Id;
                            select * from pads where pad_series_id = @Id;
                            select * from pad_options po left join pads pc on po.pad_id = pc.id where pad_series_id = @Id;
                        ",
                        new { Id = id }
                    )) {
                    // See if we even found a pad series first
                    var seriesRow = reader.ReadFirstOrDefault<PadSeriesRow>();
                    if (seriesRow == null) {
                        return null;
                    }

                    var polisherTypes = reader.Read<PadSeriesPolisherTypeRow>().Select(t => PolisherTypeUtils.Parse(
                        t.PolisherType
                    )).ToList();

                    var sizes = reader.Read<PadSizeRow>().Select(ps => new PadSize(
                        ps.Id,
                        new Measurement(ps.DiameterAmount, MeasurementUnitUtils.Parse(ps.DiameterUnit)),
                        ps.ThicknessAmount != null ? new Measurement(ps.ThicknessAmount ?? 0, MeasurementUnitUtils.Parse(ps.ThicknessUnit!)) : null
                    )).ToList();

                    var images = reader.Read<Tuple<PadImageRow, ImageRow>>();

                    var pads = new Dictionary<Guid, Pad>(
                        reader.Read<PadRow>().Select(p => {
                            var i = images.Where(i => i.Item1.PadId == p.Id).FirstOrDefault();
                            ProcessedImage? processedImage = null;

                            if (i != null) {
                                processedImage = new ProcessedImage(
                                    i.Item2.Id,
                                    i.Item2.FileName,
                                    i.Item2.MimeType,
                                    ImageUtils.LoadFromBinary(i.Item2.ImageData),
                                    ImageUtils.LoadFromBinary(i.Item2.ThumbnailData)
                                );
                            }

                            var pad = new Pad(
                                p.Id,
                                p.Name,
                                PadCategoryUtils.Parse(p.Category),
                                p.Material != null ? PadMaterialUtils.Parse(p.Material) : null,
                                p.Texture != null ? PadTextureUtils.Parse(p.Texture) : null,
                                p.Color != null ? PadColorUtils.Parse(p.Color) : null,
                                processedImage
                            );

                            return pad;
                        }).Select(c => new KeyValuePair<Guid, Pad>(c.Id, c))
                    );

                    var options = reader.Read<PadOptionRow>();
                    foreach (var opt in options) {
                        Pad? pad;

                        if (pads.TryGetValue(opt.PadId, out pad)) {
                            pad.Options.Add(new PadOption(opt.PadSizeId, opt.PartNumber));
                        }
                    }

                    return new PadSeries(
                        seriesRow.Id,
                        seriesRow.Name,
                        seriesRow.BrandId,
                        polisherTypes,
                        sizes,
                        pads.Values.ToList()
                    );
                }
            }
        }

        public async Task Add(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into pad_series (id, brand_id, name) values (@Id, @BrandId, @Name);",
                        new PadSeriesRow() {
                            Id = series.Id,
                            BrandId = series.BrandId,
                            Name = series.Name
                        }
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_series_polisher_types (pad_series_id, polisher_type) values (@PadSeriesId, @PolisherType);",
                        series.PolisherTypes.Select(pt => new PadSeriesPolisherTypeRow() {
                            PadSeriesId = series.Id,
                            PolisherType = pt.Serialize()
                        }).ToList()
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_sizes (id, pad_series_id, diameter_amount, diameter_unit, thickness_amount, thickness_unit) values (@Id, @PadSeriesId, @DiameterAmount, @DiameterUnit, @ThicknessAmount, @ThicknessUnit);",
                        series.Sizes.Select(s => new PadSizeRow() {
                            Id = s.Id,
                            PadSeriesId = series.Id,
                            DiameterAmount = s.Diameter.Amount,
                            DiameterUnit = s.Diameter.Unit.Serialize(),
                            ThicknessAmount = s.Thickness?.Amount,
                            ThicknessUnit = s.Thickness?.Unit.Serialize()
                        }).ToList()
                    );

                    List<PadImageRow> imageRelations = new();
                    List<ImageRow> images = new();

                    foreach (var pad in series.Pads) {
                        if (pad.Image == null) {
                            continue;
                        }

                        imageRelations.Add(new() { ImageId = pad.Image.Id, PadId = pad.Id });
                        images.Add(new() {
                            Id = pad.Image.Id,
                            FileName = pad.Image.FileName,
                            MimeType = pad.Image.MimeType,
                            ImageData = ImageUtils.ToBinary(pad.Image.Full),
                            ThumbnailData = ImageUtils.ToBinary(pad.Image.Thumbnail)
                        });
                    }

                    if (images.Count > 0) {
                        await conn.ExecuteAsync(
                            @"insert into images(id, parent_id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @ParentId, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                            images
                        );
                    }

                    if (imageRelations.Count > 0) {
                        await conn.ExecuteAsync(
                            @"insert into pad_images (pad_id, image_id) values (@PadId, @ImageId);",
                            imageRelations
                        );
                    }

                    await conn.ExecuteAsync(
                        @"insert into pads (id, pad_series_id, category, material, texture, color, name, image_id) values (@Id, @PadSeriesId, @Category, @Material, @Texture, @Color, @Name, @ImageId);",
                        series.Pads.Select(c => new PadRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Material = c.Material?.Serialize(),
                            Texture = c.Texture?.Serialize(),
                            Color = c.Color?.Serialize(),
                            Name = c.Name,
                            ImageId = c.Image?.Id
                        }).ToList()
                    );

                    var optionRows = new List<PadOptionRow>();
                    foreach (var color in series.Pads) {
                        foreach (var option in color.Options) {
                            optionRows.Add(new PadOptionRow() {
                                Id = Guid.NewGuid(),
                                PadId = color.Id,
                                PadSizeId = option.PadSizeId,
                                PartNumber = option.PartNumber
                            });
                        }
                    }

                    await conn.ExecuteAsync(
                        @"insert into pad_options (id, pad_id, pad_size_id, part_number) values (@Id, @PadId, @PadSizeId, @PartNumber);",
                        optionRows
                    );

                    t.Commit();
                }
            }
        }

        public async Task Update(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // pull in old one, and nuke records
                    var old = (await FindById(series.Id))!;
                    await conn.ExecuteAsync(@"delete from pad_series_polisher_types where pad_series_id = @Id", old);
                    await conn.ExecuteAsync(@"delete from pad_options where pad_id = @Id;", old.Pads);
                    await conn.ExecuteAsync(@"delete from pad_sizes where pad_series_id = @Id;", old);
                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", old);
                    await conn.ExecuteAsync(@"delete from pad_images where pad_id = @Id;", old.Pads);
                    await conn.ExecuteAsync(@"delete from images where id = @Id;", old.Pads.Where(p => p.Image != null).ToList());

                    await conn.ExecuteAsync(
                        @"update pad_series set brand_id = @BrandId, name = @Name where id = @Id;",
                        new PadSeriesRow() {
                            Id = series.Id,
                            BrandId = series.BrandId,
                            Name = series.Name
                        }
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_series_polisher_types (pad_series_id, polisher_type) values (@PadSeriesId, @PolisherType);",
                        series.PolisherTypes.Select(pt => new PadSeriesPolisherTypeRow() {
                            PadSeriesId = series.Id,
                            PolisherType = pt.Serialize()
                        }).ToList()
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_sizes (id, pad_series_id, diameter_amount, diameter_unit, thickness_amount, thickness_unit) values (@Id, @PadSeriesId, @DiameterAmount, @DiameterUnit, @ThicknessAmount, @ThicknessUnit);",
                        series.Sizes.Select(s => new PadSizeRow() {
                            Id = s.Id,
                            PadSeriesId = series.Id,
                            DiameterAmount = s.Diameter.Amount,
                            DiameterUnit = s.Diameter.Unit.Serialize(),
                            ThicknessAmount = s.Thickness?.Amount,
                            ThicknessUnit = s.Thickness?.Unit.Serialize()
                        }).ToList()
                    );

                    List<PadImageRow> imageRelations = new();
                    List<ImageRow> images = new();

                    foreach (var pad in series.Pads) {
                        if (pad.Image == null) {
                            continue;
                        }

                        imageRelations.Add(new() { ImageId = pad.Image.Id, PadId = pad.Id });
                        images.Add(new() {
                            Id = pad.Image.Id,
                            FileName = pad.Image.FileName,
                            MimeType = pad.Image.MimeType,
                            ImageData = ImageUtils.ToBinary(pad.Image.Full),
                            ThumbnailData = ImageUtils.ToBinary(pad.Image.Thumbnail)
                        });
                    }

                    if (images.Count > 0) {
                        await conn.ExecuteAsync(
                            @"insert into images(id, parent_id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @ParentId, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                            images
                        );
                    }

                    if (imageRelations.Count > 0) {
                        await conn.ExecuteAsync(
                            @"insert into pad_images (pad_id, image_id) values (@PadId, @ImageId);",
                            imageRelations
                        );
                    }

                    await conn.ExecuteAsync(
                        @"insert into pads (id, pad_series_id, category, material, texture, color, name, image_id) values (@Id, @PadSeriesId, @Category, @Material, @Texture, @Color, @Name, @ImageId);",
                        series.Pads.Select(c => new PadRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Material = c.Material?.Serialize(),
                            Texture = c.Texture?.Serialize(),
                            Color = c.Color?.Serialize(),
                            Name = c.Name,
                            ImageId = c.Image?.Id
                        }).ToList()
                    );

                    var optionRows = new List<PadOptionRow>();
                    foreach (var pad in series.Pads) {
                        foreach (var option in pad.Options) {
                            optionRows.Add(new PadOptionRow() {
                                Id = Guid.NewGuid(),
                                PadId = pad.Id,
                                PadSizeId = option.PadSizeId,
                                PartNumber = option.PartNumber
                            });
                        }
                    }

                    await conn.ExecuteAsync(
                        @"insert into pad_options (id, pad_id, pad_size_id, part_number) values (@Id, @PadId, @PadSizeId, @PartNumber);",
                        optionRows
                    );

                    t.Commit();
                }
            }
        }

        public async Task Delete(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(@"delete from pad_series_polisher_types where pad_series_id = @Id", series);
                    await conn.ExecuteAsync(@"delete from pad_options where pad_id = @Id;", series.Pads);
                    await conn.ExecuteAsync(@"delete from pad_sizes where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from images where parent_id = @Id;", series.Pads);
                    await conn.ExecuteAsync(@"delete from pad_series where id = @Id;", series);
                    t.Commit();
                }
            }
        }
    }
}