using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Persistence.Shared;
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
                            select i.* from images i join pad_colors pc on i.id = pc.image_id where pad_series_id = @Id;
                            select * from pad_colors where pad_series_id = @Id;
                            select * from pad_options po left join pad_colors pc on po.pad_color_id = pc.id where pad_series_id = @Id;
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

                    var images = reader.Read<ImageRow>();

                    var colors = new Dictionary<Guid, PadColor>(
                        reader.Read<PadColorRow>().Select(c => {
                            var imageRow = images.Where(i => i.ParentId == c.Id).FirstOrDefault();
                            ProcessedImage? processedImage = null;

                            if (imageRow != null) {
                                processedImage = new ProcessedImage(
                                    imageRow.Id,
                                    imageRow.FileName,
                                    imageRow.MimeType,
                                    ImageUtils.LoadFromBinary(imageRow.ImageData),
                                    ImageUtils.LoadFromBinary(imageRow.ThumbnailData)
                                );
                            }

                            var color = new PadColor(
                                c.Id,
                                c.Name,
                                PadCategoryUtils.Parse(c.Category),
                                processedImage
                            );

                            return color;
                        }).Select(c => new KeyValuePair<Guid, PadColor>(c.Id, c))
                    );

                    var options = reader.Read<PadOptionRow>();
                    foreach (var opt in options) {
                        PadColor? color;

                        if (colors.TryGetValue(opt.PadColorId, out color)) {
                            color.Options.Add(new PadOption(opt.PadSizeId, opt.PartNumber));
                        }
                    }

                    return new PadSeries(
                        seriesRow.Id,
                        seriesRow.Name,
                        seriesRow.BrandId,
                        PadMaterialUtils.Parse(seriesRow.Material),
                        PadTextureUtils.Parse(seriesRow.Texture),
                        polisherTypes,
                        sizes,
                        colors.Values.ToList()
                    );
                }
            }
        }

        public async Task Add(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into pad_series (id, brand_id, name, material, texture) values (@Id, @BrandId, @Name, @Material, @Texture);",
                        new PadSeriesRow() {
                            Id = series.Id,
                            BrandId = series.BrandId,
                            Name = series.Name,
                            Material = series.Material.Serialize(),
                            Texture = series.Texture.Serialize()
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

                    var imageRows = series.Colors.Where(c => c.Image != null).Select(c => new ImageRow() {
                        Id = c.Image!.Id,
                        ParentId = c.Id,
                        FileName = c.Image.FileName,
                        MimeType = c.Image.MimeType,
                        ImageData = ImageUtils.ToBinary(c.Image.Full),
                        ThumbnailData = ImageUtils.ToBinary(c.Image.Thumbnail)
                    });

                    if (imageRows.Count() > 0) {
                        await conn.ExecuteAsync(
                            @"insert into images(id, parent_id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @ParentId, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                            imageRows
                        );
                    }

                    await conn.ExecuteAsync(
                        @"insert into pad_colors (id, pad_series_id, category, name, image_id) values (@Id, @PadSeriesId, @Category, @Name, @ImageId);",
                        series.Colors.Select(c => new PadColorRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Name = c.Name,
                            ImageId = c.Image?.Id
                        }).ToList()
                    );

                    var optionRows = new List<PadOptionRow>();
                    foreach (var color in series.Colors) {
                        foreach (var option in color.Options) {
                            optionRows.Add(new PadOptionRow() {
                                Id = Guid.NewGuid(),
                                PadColorId = color.Id,
                                PadSizeId = option.PadSizeId,
                                PartNumber = option.PartNumber
                            });
                        }
                    }

                    await conn.ExecuteAsync(
                        @"insert into pad_options (id, pad_color_id, pad_size_id, part_number) values (@Id, @PadColorId, @PadSizeId, @PartNumber);",
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
                    var old = await FindById(series.Id)!;
                    await conn.ExecuteAsync(@"delete from pad_series_polisher_types where pad_series_id = @Id", old);
                    await conn.ExecuteAsync(@"delete from pad_options where pad_color_id = @Id;", old.Colors);
                    await conn.ExecuteAsync(@"delete from pad_sizes where pad_series_id = @Id;", old);
                    await conn.ExecuteAsync(@"delete from pad_colors where pad_series_id = @Id;", old);
                    await conn.ExecuteAsync(@"delete from images where parent_id = @Id;", old.Colors);

                    await conn.ExecuteAsync(
                        @"update pad_series set brand_id = @BrandId, name = @Name, material = @Material, texture = @Texture where id = @Id;",
                        new PadSeriesRow() {
                            Id = series.Id,
                            BrandId = series.BrandId,
                            Name = series.Name,
                            Material = series.Material.Serialize(),
                            Texture = series.Texture.Serialize()
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

                    var imageRows = series.Colors.Where(c => c.Image != null).Select(c => new ImageRow() {
                        Id = c.Image!.Id,
                        ParentId = c.Id,
                        FileName = c.Image.FileName,
                        MimeType = c.Image.MimeType,
                        ImageData = ImageUtils.ToBinary(c.Image.Full),
                        ThumbnailData = ImageUtils.ToBinary(c.Image.Thumbnail)
                    });

                    if (imageRows.Count() > 0) {
                        await conn.ExecuteAsync(
                            @"insert into images(id, parent_id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @ParentId, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                            imageRows
                        );
                    }

                    await conn.ExecuteAsync(
                        @"insert into pad_colors (id, pad_series_id, category, name, image_id) values (@Id, @PadSeriesId, @Category, @Name, @ImageId);",
                        series.Colors.Select(c => new PadColorRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Name = c.Name,
                            ImageId = c.Image?.Id
                        }).ToList()
                    );

                    var optionRows = new List<PadOptionRow>();
                    foreach (var color in series.Colors) {
                        foreach (var option in color.Options) {
                            optionRows.Add(new PadOptionRow() {
                                Id = Guid.NewGuid(),
                                PadColorId = color.Id,
                                PadSizeId = option.PadSizeId,
                                PartNumber = option.PartNumber
                            });
                        }
                    }

                    await conn.ExecuteAsync(
                        @"insert into pad_options (id, pad_color_id, pad_size_id, part_number) values (@Id, @PadColorId, @PadSizeId, @PartNumber);",
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
                    await conn.ExecuteAsync(@"delete from pad_options where pad_color_id = @Id;", series.Colors);
                    await conn.ExecuteAsync(@"delete from pad_sizes where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from pad_colors where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from images where parent_id = @Id;", series.Colors);
                    await conn.ExecuteAsync(@"delete from pad_series where id = @Id;", series);
                    t.Commit();
                }
            }
        }
    }
}