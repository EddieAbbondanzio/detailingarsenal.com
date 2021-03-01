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

        public async Task<PadSeries?> FindByName(string name) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                        @"  select * from pad_series where name = @Name;
                            select * from pad_sizes 
                                join pad_series ps on ps.id = pad_sizes.pad_series_id
                                where ps.name = @Name;
                            select pi.*, i.* from images i 
                                join pad_images pi on i.id = pi.image_id 
                                join pads p on pi.pad_id = p.id 
                                join pad_series ps on ps.id = p.pad_series_id
                                where ps.name = @Name;
                            select pads.* from pads
                                join pad_series ps on pads.pad_series_id = ps.id
                                where ps.name = @Name;
                            select po.* from pad_options po 
                                left join pads pc on po.pad_id = pc.id 
                                join pad_series ps on pc.pad_series_id = ps.id
                                where ps.name = @Name;
                            select po.id as pad_option_id, pn.* from part_numbers pn 
                                join pad_option_part_numbers popn on pn.id = popn.part_number_id 
                                join pad_options po on po.id = popn.pad_option_id 
                                join pads p on po.pad_id = p.id 
                                join pad_series ps on p.pad_series_id = ps.id
                                where ps.name = @Name; 
                        ",
                        new { Name = name }
                    )) {
                    // See if we even found a pad series first
                    var seriesRow = reader.ReadFirstOrDefault<PadSeriesRow>();
                    if (seriesRow == null) {
                        return null;
                    }

                    var sizes = reader.Read<PadSizeRow>().Select(ps => new PadSize(
                        ps.Id,
                        new Measurement(ps.DiameterAmount, MeasurementUnitUtils.Parse(ps.DiameterUnit)),
                        ps.ThicknessAmount != null ? new Measurement(ps.ThicknessAmount ?? 0, MeasurementUnitUtils.Parse(ps.ThicknessUnit!)) : null
                    )).ToList();

                    var images = reader.Read<PadImageRow, ImageRow, Tuple<PadImageRow, ImageRow>>((pi, i) => new Tuple<PadImageRow, ImageRow>(pi, i));

                    var padKVpairs = reader.Read<PadRow>().Select(p => {
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
                            p.Category.ToList(),
                            p.Material != null ? PadMaterialUtils.Parse(p.Material) : null,
                            p.Texture != null ? PadTextureUtils.Parse(p.Texture) : null,
                            p.Color != null ? PadColorUtils.Parse(p.Color) : null,
                            p.HasCenterHole,
                            processedImage
                        );

                        return pad;
                    }).Select(p => new KeyValuePair<Guid, Pad>(p.Id, p));

                    var pads = new Dictionary<Guid, Pad>(padKVpairs);

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
                            option.PartNumbers.Add(new PartNumber(partNumber.PartNumber.Id, partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
                        }
                    }

                    return new PadSeries(
                        seriesRow.Id,
                        seriesRow.Name,
                        seriesRow.BrandId,
                        seriesRow.PolisherTypes.ToList(),
                        sizes,
                        pads.Values.ToList()
                    );
                }
            }
        }

        public async Task<PadSeries?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                        @"  select * from pad_series where id = @Id;
                            select * from pad_sizes where pad_series_id = @Id;
                            select pi.*, i.* from images i join pad_images pi on i.id = pi.image_id join pads p on pi.pad_id = p.id where p.pad_series_id = @Id;
                            select pads.* from pads where pad_series_id = @Id;
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
                                p.Category.ToList(),
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
                            option.PartNumbers.Add(new PartNumber(partNumber.PartNumber.Id, partNumber.PartNumber.Value, partNumber.PartNumber.Notes));
                        }
                    }

                    return new PadSeries(
                        seriesRow.Id,
                        seriesRow.Name,
                        seriesRow.BrandId,
                        seriesRow.PolisherTypes.ToList(),
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
                        @"insert into pad_series (id, brand_id, name, polisher_types) values (@Id, @BrandId, @Name, @PolisherTypes);",
                        new PadSeriesRow() {
                            Id = series.Id,
                            BrandId = series.BrandId,
                            Name = series.Name,
                            PolisherTypes = Flatten(series.PolisherTypes)
                        }
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
                            Category = Flatten(c.Category),
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

                    // Update parent record.
                    await conn.ExecuteAsync(
                        @"update pad_series set brand_id = @BrandId, name = @Name, polisher_types = @PolisherTypes where id = @Id;",
                        new PadSeriesRow() {
                            Id = series.Id,
                            BrandId = series.BrandId,
                            Name = series.Name,
                            PolisherTypes = Flatten(series.PolisherTypes)
                        }
                    );

                    await UpdatePadSizes(conn, series.Id, old.Sizes, series.Sizes);
                    await UpdatePads(conn, series.Id, old.Pads, series.Pads);

                    t.Commit();
                }
            }
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
                newSizes.Select(s => new PadSizeRow() {
                    Id = s.Id,
                    DiameterAmount = s.Diameter.Amount,
                    DiameterUnit = s.Diameter.Unit.Serialize(),
                    ThicknessAmount = s.Thickness?.Amount,
                    ThicknessUnit = s.Thickness?.Unit.Serialize(),
                    PadSeriesId = padSeriesId
                }).ToList()
            );
        }

        async Task UpdatePads(IDbConnection conn, Guid padSeriesId, List<Pad> oldPads, List<Pad> newPads) {
            // Remove pads that were deleted from the pad series, but are still in the database.
            var deletedPads = oldPads.Where(s => !newPads.Any(np => np.Id == s.Id)).ToList();

            // Can't leave part_number oprhans
            var deletedPartNumberIds = await conn.QueryAsync<Guid>(@"
                select pn.id from part_numbers pn 
                    join pad_option_part_numbers pnpc on pnpc.part_number_id = pn.id
                    join pad_options po on pnpc.pad_option_id = po.id
                    join pads p on po.pad_id = p.id
                    where p.id = any(@Ids);
                
                ", new { Ids = deletedPads.Select(dp => dp.Id).ToArray() });

            // Can't leave image orhpans
            var deletedImageIds = await conn.QueryAsync<Guid>(@"
                select i.id from images i
                    join pad_images pi on i.id = pi.image_id
                    where pi.pad_id = any(@Ids);"
                    , new { Ids = deletedPads.Select(dp => dp.Id).ToArray() }
            );

            await conn.ExecuteAsync(@"delete from pads where id = @Id;", deletedPads); //TODO: This will break if a pad has reviews
            await conn.ExecuteAsync(@"delete from part_numbers where id = @Id;", deletedPartNumberIds.Select(i => new { Id = i }).ToList());
            await conn.ExecuteAsync(@"delete from images where id = @Id;", deletedImageIds.Select(i => new { Id = i }).ToList());

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
                newPads.Select(p => new PadRow() {
                    Id = p.Id,
                    Name = p.Name,
                    Category = Flatten(p.Category),
                    Color = p.Color?.Serialize(),
                    HasCenterHole = p.HasCenterHole,
                    Material = p.Material?.Serialize(),
                    PadSeriesId = padSeriesId,
                    Texture = p.Texture?.Serialize(),
                })
            );

            List<PadImageRow> imageRelations = new();
            List<ImageRow> images = new();

            // Upsert pad options
            foreach (var pad in newPads) {
                // On image change, we gotta kill the orphan.
                var oldImageId = oldPads.Find(p => p.Id == pad.Id)?.Image?.Id;
                if (oldImageId != null && oldImageId != pad.Image?.Id) {
                    // Cascade will handle pad_images record.
                    await conn.ExecuteAsync(@"delete from images where id = @Id;", new { Id = oldImageId });
                }

                List<Guid> deletedPartNumbers = new();
                IEnumerable<PartNumber> newPartNumbers = newPads.SelectMany(p => p.Options.SelectMany(o => o.PartNumbers));

                foreach (var pn in oldPads.SelectMany(p => p.Options.SelectMany(o => o.PartNumbers))) {
                    if (!newPartNumbers.Any(newPn => newPn.Id == pn.Id)) {
                        deletedPartNumbers.Add(pn.Id);
                    }
                }

                if (deletedPartNumbers.Count > 0) {
                    await conn.ExecuteAsync(@"delete from part_numbers where id = any(@Ids);", new { Ids = deletedPartNumbers.ToArray() });
                }

                if (pad.Image != null) {
                    imageRelations.Add(new() { ImageId = pad.Image.Id, PadId = pad.Id });
                    images.Add(new() {
                        Id = pad.Image.Id,
                        FileName = pad.Image.FileName,
                        MimeType = pad.Image.MimeType,
                        ImageData = ImageUtils.ToBinary(pad.Image.Full),
                        ThumbnailData = ImageUtils.ToBinary(pad.Image.Thumbnail)
                    });
                }

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
                        new PadOptionRow() {
                            Id = option.Id,
                            PadId = pad.Id,
                            PadSizeId = option.PadSizeId
                        }
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
                        new PartNumberRow() {
                            Id = partNumber.Id,
                            Notes = partNumber.Notes,
                            Value = partNumber.Value
                        });

                        await conn.ExecuteAsync(@"
                        insert into pad_option_part_numbers (
                            pad_option_id,
                            part_number_id
                        ) values (
                            @PadOptionId,
                            @PartNumberId
                        ) on conflict (pad_option_id, part_number_id) do update set
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
                    @"insert into images(
                        id, 
                        file_name, 
                        mime_type, 
                        image_data, 
                        thumbnail_data
                    ) values (
                        @Id, 
                        @FileName, 
                        @MimeType, 
                        @ImageData, 
                        @ThumbnailData
                    ) on conflict (id) do update set
                        file_name = @FileName,
                        mime_type = @MimeType,
                        image_data = @ImageData,
                        thumbnail_data = @ThumbnailData;",
                    images
                );

                await conn.ExecuteAsync(
                    @"insert into pad_images(
                        pad_id,
                        image_id
                    ) values (
                        @PadId,
                        @ImageId
                    ) on conflict (pad_id, image_id) do nothing;", //No id on pad_images table 
                    imageRelations
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

        PadCategoryBitwise Flatten(List<PadCategory> category) {
            var bitwise = PadCategoryBitwise.None;

            for (int i = 0; i < category.Count; i++) {
                switch (category[i]) {
                    case PadCategory.Cutting:
                        bitwise |= PadCategoryBitwise.Cutting;
                        break;
                    case PadCategory.Polishing:
                        bitwise |= PadCategoryBitwise.Polishing;
                        break;
                    case PadCategory.Finishing:
                        bitwise |= PadCategoryBitwise.Finishing;
                        break;
                }
            }

            return bitwise;
        }

        PolisherTypeBitwise Flatten(List<PolisherType> polisherTypes) {
            var bitwise = PolisherTypeBitwise.None;

            for (int i = 0; i < polisherTypes.Count; i++) {
                switch (polisherTypes[i]) {
                    case PolisherType.DualAction:
                        bitwise |= PolisherTypeBitwise.DualAction;
                        break;
                    case PolisherType.LongThrow:
                        bitwise |= PolisherTypeBitwise.LongThrow;
                        break;
                    case PolisherType.ForcedRotation:
                        bitwise |= PolisherTypeBitwise.ForcedRotation;
                        break;
                    case PolisherType.Mini:
                        bitwise |= PolisherTypeBitwise.Mini;
                        break;
                    case PolisherType.Rotary:
                        bitwise |= PolisherTypeBitwise.Rotary;
                        break;
                }
            }

            return bitwise;
        }
    }
}