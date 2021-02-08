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
using System.Data;

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
                            select po.* from pad_options po left join pads pc on po.pad_id = pc.id where pad_series_id = @Id;
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
                    // Pull in the OG record. useful to see what was created or updated.
                    var old = (await FindById(series.Id))!;

                    // Update polisher types




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

                    await conn.ExecuteAsync(@"delete from pad_option_part_numbers where pad_option_id = @Id", padOptionIds);
                    await conn.ExecuteAsync(@"delete from part_numbers where id = @Id;", partNumberIds);

                    var padImageIds = (await conn.QueryAsync<Guid>(@"
                        select i.id from images i
                            join pad_images pi on pi.image_id = i.id
                            join pads p on pi.pad_id = p.id
                            where p.pad_series_id = @Id;
                    ", series)).Select(id => new { Id = id }).ToList();

                    await conn.ExecuteAsync(@"delete from pad_options where pad_id = @Id;", old.Pads);
                    await conn.ExecuteAsync(@"delete from pad_images where pad_id = @Id;", old.Pads);
                    await conn.ExecuteAsync(@"delete from images where id = @Id;", padImageIds);

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

        /// <summary>
        /// Update the recommended polisher types for a pad series.
        /// </summary>
        /// <param name="conn">Active database connection.</param>
        /// <param name="padSeriesId">Id of the pad series parent record.</param>
        /// <param name="polisherTypes">The list of polisher types to save.</param>
        async Task UpdatePolisherTypes(IDbConnection conn, Guid padSeriesId, List<PolisherType> polisherTypes) {
            await conn.ExecuteAsync(@"delete from pad_series_polisher_types where pad_series_id = @Id", new { Id = padSeriesId });
            await conn.ExecuteAsync(
                @"insert into pad_series_polisher_types (pad_series_id, polisher_type) values (@PadSeriesId, @PolisherType);",
                polisherTypes.Select(pt => new PadSeriesPolisherTypeRow() {
                    PadSeriesId = padSeriesId,
                    PolisherType = pt.Serialize()
                }).ToList()
            );
        }

        /// <summary>
        /// Update the pad sizes of a pad series.
        /// </summary>
        /// <param name="conn">The active database connection.</param>
        /// <param name="padSeriesId">Id of the pad series parent record.</param>
        /// <param name="oldSizes">The (not yet updated) sizes we pulled from the database.</param>
        /// <param name="newSizes">The new sizes to be stored.</param>
        async Task UpdatePadSizes(IDbConnection conn, Guid padSeriesId, List<PadSize> oldSizes, List<PadSize> newSizes) {
            // Remove sizes that were deleted from the pad series, but are still in the database.
            var deletedSizes = oldSizes.Where(s => !newSizes.Any(ns => ns.Id == s.Id)).ToList();
            await conn.ExecuteAsync(@"delete from pad_sizes where id = @Id;", deletedSizes);

            // Insert, or update remaining sizes
            await conn.ExecuteAsync(@"
                insert into pad_sizes (
                    id,
                    pad_series_id,
                    diameter_amount,
                    diameter_unit,
                    thickness_amount,
                    thickness_unit
                ) values (
                    @Id,
                    @PadSeriesId,
                    @DiameterAmount,
                    @DiameterUnit,
                    @ThicknessAmount,
                    @ThicknessUnit
                ) on conflict (id) do update set
                    pad_series_id = @PadSeriesId,
                    diameter_amount = @DiameterAmount,
                    diameter_unit = @DiameterUnit,
                    thickness_amount = @ThicknessAmount,
                    thickness_unit = @ThicknessUnit;
                ",
                newSizes
            );
        }

        async Task UpdatePads(IDbConnection conn, Guid padSeriesId, List<Pad> oldPads, List<Pad> newPads) {
            // Remove pads that were deleted from the pad series, but are still in the database.
            var deletedPads = oldPads.Where(s => !newPads.Any(np => np.Id == s.Id)).ToList();

            // Can't leave part_number oprhans
            var deletedPartNumberIds = await conn.QueryAsync<Guid>(@"
                select pn.id from part_numbers pn 
                    join pad_option_part_numbers pnpc on pn.part_number_id
                    join pad_options po on pnpc.pad_option_id = po.id
                    join pads p on po.pad_id = p.id
                    where p.id = @Id;
                
                ", deletedPads);

            await conn.ExecuteAsync(@"delete from part_numbers where id = @Id;", deletedPartNumberIds.Select(i => new { Id = i }).ToList());
            await conn.ExecuteAsync(@"delete from pads where id = @Id;", deletedPads); //TODO: This will break if a pad has reviews

            // Upsert remaining pads
            await conn.ExecuteAsync(@"
                insert into pads (
                        id, 
                        pad_series_id, 
                        category, 
                        material, 
                        texture, 
                        color, 
                        has_center_hole, 
                        name
                    ) values (
                        @Id, 
                        @PadSeriesId, 
                        @Category, 
                        @Material, 
                        @Texture, 
                        @Color, 
                        @HasCenterHole, 
                        @Name
                    ) on conflict (id) do update set
                        pad_series_id = @PadSeriesId,
                        category = @Category,
                        material = @Material,
                        texture = @Texture,
                        color = @Color,
                        has_center_hole = @HasCenterHole,
                        name = @Name;
                ",
                newPads
            );

            List<PadImageRow> imageRelations = new();
            List<ImageRow> images = new();

            // Upsert pad options
            foreach (var pad in newPads) {
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

                foreach (var option in pad.Options) {
                    await conn.ExecuteAsync(@"
                        insert into pad_options (
                            id,
                            pad_id,
                            pad_size_id
                        ) values (
                            @Id,
                            @PadId,
                            @PadSizeId
                        ) on conflict (id) do update set
                            pad_id = @PadId,
                            pad_size_id = @PadSizeId;
                        ",
                        option
                    );

                    foreach (var partNumber in option.PartNumbers) {
                        await conn.ExecuteAsync(@"
                        insert into part_numbers (
                            id,
                            value,
                            notes
                        ) values (
                            @Id,
                            @Value,
                            @Notes
                        ) on conflict (id) do update set
                        value = @Value,
                        notes = @Notes;
                        ",
                        partNumber);

                        await conn.ExecuteAsync(@"
                        insert into pad_option_part_numbers (
                            pad_option_id,
                            part_number_id
                        ) values (
                            @PadOptionId,
                            @PartNumberId
                        ) on conflict (part_number_id) do update set
                        pad_option_id = @PadOptionId;
                        ",
                        new PadOptionPartNumberRow() {
                            PadOptionId = option.Id,
                            PartNumberId = partNumber.Id
                        });
                    }
                }
            }

            if (images.Count > 0) {
                await conn.ExecuteAsync(
                    @"insert into images(id, file_name, mime_type, image_data, thumbnail_data) values (@Id, @FileName, @MimeType, @ImageData, @ThumbnailData);",
                    images
                );
            }
        }

        public async Task Delete(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // Thank you cascade
                    await conn.ExecuteAsync(@"delete from pad_series where id = @Id;", series);
                    t.Commit();
                }
            }
        }
    }
}