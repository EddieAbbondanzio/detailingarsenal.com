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
                            select po.id as pad_option_id, pn.* from part_numbers pn join pad_option_part_numbers popn on pn.id = popn.part_number_id join pad_options po on po.id = popn.pad_option_id join pads p on po.pad_id = p.id where p.pad_series_id = @Id; 
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

                    var images = reader.Read<PadImageRow, ImageRow, Tuple<PadImageRow, ImageRow>>((pi, i) => new Tuple<PadImageRow, ImageRow>(pi, i));

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
                                p.HasCenterHole,
                                processedImage
                            );

                            return pad;
                        }).Select(c => new KeyValuePair<Guid, Pad>(c.Id, c))
                    );

                    var options = reader.Read<PadOptionRow>();
                    var optionDict = new Dictionary<Guid, PadOption>();
                    foreach (var opt in options) {
                        Pad? pad;

                        if (pads.TryGetValue(opt.PadId, out pad)) {
                            var po = new PadOption(opt.PadSizeId);
                            pad.Options.Add(po);
                            optionDict.Add(opt.Id, po);
                        }
                    }

                    var partNumbers = reader.Read<Guid, PartNumberRow, (Guid PadOptionId, PartNumberRow PartNumber)>((id, pn) => (id, pn));
                    foreach (var partNumber in partNumbers) {
                        PadOption? option;

                        if (optionDict.TryGetValue(partNumber.PadOptionId, out option)) {
                            option.PartNumbers.Add(new PartNumber(partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
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
                            @"insert into images(id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                            images
                        );
                    }

                    await conn.ExecuteAsync(
                        @"insert into pads (id, pad_series_id, category, material, texture, color, has_center_hole, name) values (@Id, @PadSeriesId, @Category, @Material, @Texture, @Color, @HasCenterHole, @Name);",
                        series.Pads.Select(c => new PadRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Material = c.Material?.Serialize(),
                            Texture = c.Texture?.Serialize(),
                            Color = c.Color?.Serialize(),
                            HasCenterHole = c.HasCenterHole,
                            Name = c.Name,
                        }).ToList()
                    );

                    if (imageRelations.Count > 0) {
                        await conn.ExecuteAsync(
                            @"insert into pad_images (pad_id, image_id) values (@PadId, @ImageId);",
                            imageRelations
                        );
                    }

                    List<PadOptionRow> padOptionRows = new();
                    List<PadOptionPartNumberRow> padOptionPartNumberRows = new();
                    List<PartNumberRow> partNumberRows = new();

                    foreach (var color in series.Pads) {
                        foreach (var option in color.Options) {
                            var optionRow = new PadOptionRow() {
                                Id = Guid.NewGuid(),
                                PadId = color.Id,
                                PadSizeId = option.PadSizeId,
                            };

                            padOptionRows.Add(optionRow);

                            foreach (var partNumber in option.PartNumbers) {
                                var partNumberRow = new PartNumberRow() {
                                    Id = Guid.NewGuid(),
                                    Value = partNumber.Value,
                                    Notes = partNumber.Notes
                                };

                                padOptionPartNumberRows.Add(new PadOptionPartNumberRow() {
                                    PadOptionId = optionRow.Id,
                                    PartNumberId = partNumberRow.Id,
                                });

                                partNumberRows.Add(partNumberRow);
                            }
                        }
                    }

                    await conn.ExecuteAsync(@"insert into pad_options (id, pad_id, pad_size_id) values (@Id, @PadId, @PadSizeId);", padOptionRows);
                    await conn.ExecuteAsync(@"insert into part_numbers (id, value, notes) values (@Id, @Value, @Notes);", partNumberRows);
                    await conn.ExecuteAsync(@"insert into pad_option_part_numbers (pad_option_id, part_number_id) values (@PadOptionId, @PartNumberId);", padOptionPartNumberRows);

                    t.Commit();
                }
            }
        }

        public async Task Update(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // pull in old one, and nuke records
                    var old = (await FindById(series.Id))!;

                    // get part number ids
                    var partNumberIds = (await conn.QueryAsync<Guid>(@"
                        select pn.id from part_numbers pn 
                            join pad_option_part_numbers popn on pn.id = popn.part_number_id 
                            join pad_options po on popn.pad_option_id = po.id 
                            join pads p on po.pad_id = p.id 
                            where p.pad_series_id = @Id;
                        ", old)).Select(id => new { Id = id }).ToList();

                    var padOptionIds = (await conn.QueryAsync<Guid>(@"
                        select po.id from pad_options po
                            join pads p on po.pad_id = p.id
                            where p.pad_series_id = @Id;
                    ", series)).Select(id => new { Id = id }).ToList();

                    await conn.ExecuteAsync(@"delete from pad_series_polisher_types where pad_series_id = @Id", old);
                    await conn.ExecuteAsync(@"delete from pad_option_part_numbers where pad_option_id = @Id", padOptionIds);
                    await conn.ExecuteAsync(@"delete from pad_options where pad_id = @Id;", old.Pads);
                    await conn.ExecuteAsync(@"delete from part_numbers where id = @Id;", partNumberIds);
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
                            @"insert into images(id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                            images
                        );
                    }

                    await conn.ExecuteAsync(
                        @"insert into pads (id, pad_series_id, category, material, texture, color, has_center_hole, name) values (@Id, @PadSeriesId, @Category, @Material, @Texture, @Color, @HasCenterHole, @Name);",
                        series.Pads.Select(p => new PadRow() {
                            Id = p.Id,
                            PadSeriesId = series.Id,
                            Category = p.Category.Serialize(),
                            Material = p.Material?.Serialize(),
                            Texture = p.Texture?.Serialize(),
                            Color = p.Color?.Serialize(),
                            HasCenterHole = p.HasCenterHole,
                            Name = p.Name,
                        }).ToList()
                    );

                    if (imageRelations.Count > 0) {
                        await conn.ExecuteAsync(
                            @"insert into pad_images (pad_id, image_id) values (@PadId, @ImageId);",
                            imageRelations
                        );
                    }

                    List<PadOptionRow> padOptionRows = new();
                    List<PadOptionPartNumberRow> padOptionPartNumberRows = new();
                    List<PartNumberRow> partNumberRows = new();

                    foreach (var color in series.Pads) {
                        foreach (var option in color.Options) {
                            var optionRow = new PadOptionRow() {
                                Id = Guid.NewGuid(),
                                PadId = color.Id,
                                PadSizeId = option.PadSizeId,
                            };

                            padOptionRows.Add(optionRow);

                            foreach (var partNumber in option.PartNumbers) {
                                var partNumberRow = new PartNumberRow() {
                                    Id = Guid.NewGuid(),
                                    Value = partNumber.Value,
                                    Notes = partNumber.Notes
                                };

                                padOptionPartNumberRows.Add(new PadOptionPartNumberRow() {
                                    PadOptionId = optionRow.Id,
                                    PartNumberId = partNumberRow.Id,
                                });

                                partNumberRows.Add(partNumberRow);
                            }
                        }
                    }

                    await conn.ExecuteAsync(@"insert into pad_options (id, pad_id, pad_size_id) values (@Id, @PadId, @PadSizeId);", padOptionRows);
                    await conn.ExecuteAsync(@"insert into part_numbers (id, value, notes) values (@Id, @Value, @Notes);", partNumberRows);
                    await conn.ExecuteAsync(@"insert into pad_option_part_numbers (pad_option_id, part_number_id) values (@PadOptionId, @PartNumberId);", padOptionPartNumberRows);

                    t.Commit();
                }
            }
        }

        public async Task Delete(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    /*
                    * Postgres doesn't support joins in deletes
                    */

                    var partNumberIds = (await conn.QueryAsync<Guid>(@"
                        select pn.id from part_numbers pn 
                            join pad_option_part_numbers popn on pn.id = popn.part_number_id 
                            join pad_options po on popn.pad_option_id = po.id 
                            join pads p on po.pad_id = p.id 
                            where p.pad_series_id = @Id;",
                        series
                    )).Select(id => new { Id = id }).ToList();

                    var padOptionIds = (await conn.QueryAsync<Guid>(@"
                        select po.id from pad_options po
                            join pads p on po.pad_id = p.id
                            where p.pad_series_id = @Id;
                    ", series)).Select(id => new { Id = id }).ToList();

                    var padImageIds = (await conn.QueryAsync<Guid>(@"
                        select i.id from images i
                            join pad_images pi on pi.image_id = i.id
                            join pads p on pi.pad_id = p.id
                            where p.pad_series_id = @Id;
                    ", series)).Select(id => new { Id = id }).ToList();

                    await conn.ExecuteAsync(@"delete from pad_series_polisher_types where pad_series_id = @Id", series);
                    await conn.ExecuteAsync(@"delete from pad_option_part_numbers where pad_option_id = @Id", padOptionIds);
                    await conn.ExecuteAsync(@"delete from part_numbers where id = @Id;", partNumberIds);
                    await conn.ExecuteAsync(@"delete from pad_options where pad_id = @Id;", series.Pads);
                    await conn.ExecuteAsync(@"delete from pad_sizes where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from pad_images where pad_id = @Id;", series.Pads);
                    await conn.ExecuteAsync(@"delete from images where id = @Id;", padImageIds);
                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from pad_series where id = @Id;", series);
                    t.Commit();
                }
            }
        }
    }
}