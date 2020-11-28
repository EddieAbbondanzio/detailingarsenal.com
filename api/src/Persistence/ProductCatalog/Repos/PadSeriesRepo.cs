using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesRepo : DatabaseInteractor, IPadSeriesRepo {
        public PadSeriesRepo(IDatabase database) : base(database) { }

        public async Task<PadSeries?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"
                        select * from pad_series where id = @Id;
                        select * from pad_series_sizes where pad_series_id = @Id;
                        select * from pads where pad_series_id = @Id;
                        select ppt.* from pad_polisher_types ppt join pads p on ppt.pad_id = p.id where p.pad_series_id = @Id;
                    ",
                    new { Id = id }
                )) {
                    // See if we even found a pad series first
                    var seriesRow = reader.ReadFirstOrDefault<PadSeriesRow>();
                    if (seriesRow == null) {
                        return null;
                    }

                    var sizes = reader.Read<PadSeriesSizeRow>().Select(pssr => new PadSeriesSize(pssr.Diameter, pssr.Thickness, pssr.PartNumber)).ToList();

                    var pads = reader.Read<PadRow>().Select(p => new Pad(
                       p.Id, p.Name, PadCategoryUtils.Parse(p.Category), PadMaterialUtils.Parse(p.Material), PadTextureUtils.Parse(p.Texture)
                    )).ToList();

                    var padLookup = pads.ToDictionary(p => p.Id, p => p);

                    // Assign each pad their polisher types
                    var padPolisherTypes = reader.Read<PadPolisherTypeRow>();
                    foreach (var padPolisherType in padPolisherTypes) {
                        Pad? pad;

                        if (padLookup.TryGetValue(padPolisherType.PadId, out pad)) {
                            pad.PolisherTypes.Add(PolisherTypeUtils.Parse(padPolisherType.PolisherType));
                        }
                    }

                    return new PadSeries(seriesRow.Id, seriesRow.Name, seriesRow.BrandId, sizes, pads);
                }
            }
        }

        public async Task Add(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // pads
                    // pad_polisher_types
                    // pad_sizes

                    // Insert the parent row for the pad series first
                    await conn.ExecuteAsync(
                        @"insert into pad_series (id, brand_id, name) values (@Id, @BrandId, @Name);", series
                    );

                    // Then insert the pad row
                    await conn.ExecuteAsync(
                        @"insert into pads 
                            (id, pad_series_id, name, category, material, texture, image_name, image_data)
                            values (@Id, @PadSeriesId, @Name, @Category, @Material, @Texture, @ImageName, @ImageData);",
                        series.Pads.Select(p => new PadRow() {
                            Id = p.Id,
                            PadSeriesId = series.Id,
                            Name = p.Name,
                            Category = p.Category.Serialize(),
                            Material = p.Material.Serialize(),
                            Texture = p.Texture.Serialize(),
                            ImageName = p.Image?.Name,
                            ImageData = p.Image?.ToBinary()
                        }).ToList()
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_polisher_types (pad_id, polisher_type) values (@PadId, @PolisherType);",
                        series.Pads.SelectMany(
                            p => p.PolisherTypes.Select(
                                pt => new PadPolisherTypeRow() { PadId = p.Id, PolisherType = pt.Serialize() }
                            )
                        ).ToList()
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_series_sizes (pad_series_id, diameter, thickness, part_number) values (@PadSeriesId, @Diameter, @Thickness, @PartNumber);",
                        series.Sizes.Select(
                                ps => new PadSeriesSizeRow() { PadSeriesId = series.Id, Diameter = ps.Diameter, Thickness = ps.Thickness, PartNumber = ps.PartNumber }
                        ).ToList()
                    );

                    t.Commit();
                }
            }
        }

        public async Task Update(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update pad_series set brand_id = @BrandId, name = @Name where id = @Id;", series
                    );

                    await conn.ExecuteAsync(@"delete from pad_polisher_types where pad_id = @Id", series.Pads);
                    await conn.ExecuteAsync(@"delete from pad_series_sizes where pad_series_id = @Id", series);
                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", series);

                    await conn.ExecuteAsync(
                                           @"insert into pads 
                            (id, pad_series_id, name, category, material, texture, image_name, image_data)
                            values (@Id, @PadSeriesId, @Name, @Category, @Material, @Texture, @ImageName, @ImageData);",
                                           series.Pads.Select(p => new PadRow() {
                                               Id = p.Id,
                                               PadSeriesId = series.Id,
                                               Name = p.Name,
                                               Category = p.Category.Serialize(),
                                               Material = p.Material.Serialize(),
                                               Texture = p.Texture.Serialize(),
                                               ImageName = p.Image?.Name,
                                               ImageData = p.Image?.ToBinary()
                                           }).ToList()
                                       );

                    await conn.ExecuteAsync(
                        @"insert into pad_polisher_types (pad_id, polisher_type) values (@PadId, @PolisherType);",
                        series.Pads.SelectMany(
                            p => p.PolisherTypes.Select(
                                pt => new PadPolisherTypeRow() { PadId = p.Id, PolisherType = pt.Serialize() }
                            )
                        ).ToList()
                    );

                    await conn.ExecuteAsync(
                        @"insert into pad_series_sizes (pad_series_id, diameter, thickness, part_number) values (@PadSeriesId, @Diameter, @Thickness, @PartNumber);",
                        series.Sizes.Select(
                                ps => new PadSeriesSizeRow() { PadSeriesId = series.Id, Diameter = ps.Diameter, Thickness = ps.Thickness, PartNumber = ps.PartNumber }
                        ).ToList()
                    );

                    t.Commit();
                }
            }
        }

        public async Task Delete(PadSeries series) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // Delete out pads first due to foreign key
                    await conn.ExecuteAsync(@"delete from pad_polisher_types where pad_id = @Id", series.Pads);
                    await conn.ExecuteAsync(@"delete from pads where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from pad_series_sizes where pad_series_id = @Id", series);
                    await conn.ExecuteAsync(@"delete from pad_series where id = @Id;", series);

                    t.Commit();
                }
            }
        }
    }
}