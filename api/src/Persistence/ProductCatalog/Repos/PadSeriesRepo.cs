using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Common;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesRepo : DatabaseInteractor, IPadSeriesRepo {
        public PadSeriesRepo(IDatabase database) : base(database) { }

        public async Task<PadSeries?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                        @"  select * from pad_series where id = @Id;
                            select * from pad_series_polisher_types where pad_series_id = @Id;
                            select * from pad_sizes where pad_series_id = @Id;
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

                    var colors = new Dictionary<Guid, PadColor>(
                        reader.Read<PadColorRow>().Select(c => new PadColor(
                            c.Name,
                            PadCategoryUtils.Parse(c.Category),
                            c.ImageName != null ? new DataUrlImage(c.ImageName, c.ImageData!) : null
                        )).Select(c => new KeyValuePair<Guid, PadColor>(c.Id, c))
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

                    await conn.ExecuteAsync(
                        @"insert into pad_colors (id, pad_series_id, category, name, image_name, image_data) values (@Id, @PadSeriesId, @Category, @Name, @ImageName, @ImageData);",
                        series.Colors.Select(c => new PadColorRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Name = c.Name,
                            ImageName = c.Image?.Name,
                            ImageData = c.Image?.ToBinary()
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

                    await conn.ExecuteAsync("delete from pad_series_polisher_types where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(
                        @"insert into pad_series_polisher_types (pad_series_id, polisher_type) values (@PadSeriesId, @PolisherType);",
                        series.PolisherTypes.Select(pt => new PadSeriesPolisherTypeRow() {
                            PadSeriesId = series.Id,
                            PolisherType = pt.Serialize()
                        }).ToList()
                    );

                    await conn.ExecuteAsync("delete from pad_sizes where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(
                        @"insert into pad_sizes (id, pad_series_id, diameter, thickness) values (@Id, @PadSeriesId, @Diameter, @Thickness);",
                        series.Sizes.Select(s => new PadSizeRow() {
                            Id = s.Id,
                            PadSeriesId = series.Id,
                            DiameterAmount = s.Diameter.Amount,
                            DiameterUnit = s.Diameter.Unit.Serialize(),
                            ThicknessAmount = s.Thickness?.Amount,
                            ThicknessUnit = s.Thickness?.Unit.Serialize()
                        }).ToList()
                    );

                    await conn.ExecuteAsync("delete from pad_options join pad_colors on pad_options.pad_colors_id = pad_colors.id where pad_series_id = @Id", series);
                    await conn.ExecuteAsync("delete from pad_colors where pad_series_id = @Id;", series);

                    await conn.ExecuteAsync(
                        @"insert into pad_colors (id, pad_series_id, category, name, image_name, image_data) values (@Id, @PadSeriesId, @Category, @Name, @ImageName, @ImageData);",
                        series.Colors.Select(c => new PadColorRow() {
                            Id = c.Id,
                            PadSeriesId = series.Id,
                            Category = c.Category.Serialize(),
                            Name = c.Name,
                            ImageName = c.Image?.Name,
                            ImageData = c.Image?.ToBinary()
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
                    await conn.ExecuteAsync(@"delete from pad_sizes where pad_series_id = @Id;", series);
                    await conn.ExecuteAsync(@"delete from pad_options where id = @Id;", series.Colors.SelectMany(c => c.Options).ToList());
                    await conn.ExecuteAsync(@"delete from pad_colors where pad_series_id = @Id;", series);

                    t.Commit();
                }
            }
        }
    }
}