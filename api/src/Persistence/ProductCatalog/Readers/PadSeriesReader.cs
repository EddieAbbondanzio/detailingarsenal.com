using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesReader : DatabaseInteractor, IPadSeriesReader {
        public PadSeriesReader(IDatabase database) : base(database) { }

        public async Task<PadSeriesReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"  select * from pad_series ps join brands b on ps.brand_id = b.id where ps.id = @Id;
                        select * from pad_series_polisher_types where pad_series_id = @Id;
                        select * from pad_sizes where pad_series_id = @Id;
                        select count(reviews.*) as count, pad_colors.id from pad_colors
                            left join reviews on reviews.pad_color_id = pad_colors.id 
                            where pad_series_id = @Id;
                        select pc.*, avg(r.cut) as cut, avg(r.finish) as finish, coalesce(avg(r.stars), 0) as stars from pad_colors pc 
                            left join reviews r on pc.id = r.pad_color_id 
                            where pad_series_id = @Id group by pc.id;
                        select * from pad_options po left join pad_colors pc on po.pad_color_id = pc.id where pad_series_id = @Id;
                    ",
                    new { Id = id }
                )) {
                    var series = reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                        (ps, b) => new PadSeriesReadModel(
                            ps.Id,
                            ps.Name,
                            new BrandReadModel(
                                b.Id,
                                b.Name
                            ),
                            ps.Material,
                            ps.Texture
                        )).ElementAt(0);

                    series.PolisherTypes.AddRange(reader.Read<PadSeriesPolisherTypeRow>()
                        .Select(p => p.PolisherType));

                    series.Sizes.AddRange(reader.Read<PadSizeReadModel>()
                        .Select(s => new PadSizeReadModel(s.Diameter, s.Thickness)));

                    var reviewCount = reader.ReadFirstOrDefault<int>();

                    var colors = new Dictionary<Guid, PadColorReadModel>(
                        reader.Read().Select(c => new KeyValuePair<Guid, PadColorReadModel>(
                            c.id,
                            new PadColorReadModel(
                                c.id,
                                c.name,
                                c.category,
                                c.image_name != null ? new DataUrlImage(c.image_name, c.image_data) : null,
                                new List<PadOptionReadModel>(),
                                c.cut,
                                c.finish,
                                new RatingReadModel(c.stars, reviewCount)
                            )
                        )
                    ));

                    var options = reader.Read<PadOptionRow>();
                    foreach (var opt in options) {
                        PadColorReadModel? color;

                        if (colors.TryGetValue(opt.PadColorId, out color)) {
                            color.Options.Add(new PadOptionReadModel(opt.PadSizeId, opt.PartNumber));
                        }
                    }

                    series.Colors.AddRange(colors.Values);
                    return series;
                }
            }
        }

        public async Task<List<PadSeriesReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"  select * from pad_series ps join brands b on ps.brand_id = b.id;
                        select * from pad_series_polisher_types;
                        select * from pad_sizes;
                        select count(reviews.*) as count, pad_colors.id from pad_colors
                            left join reviews on reviews.pad_color_id = pad_colors.id 
                            group by pad_colors.id;
                        select pc.*, avg(r.cut) as cut, avg(r.finish) as finish, coalesce(avg(r.stars), 0) as stars from pad_colors pc 
                            left join reviews r on pc.id = r.pad_color_id 
                            group by pc.id;
                        select * from pad_options po left join pad_colors pc on po.pad_color_id = pc.id;
                        "
                )) {
                    var series = new Dictionary<Guid, PadSeriesReadModel>(
                        reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                            (ps, b) => new PadSeriesReadModel(
                                ps.Id,
                                ps.Name,
                                new BrandReadModel(
                                    b.Id,
                                    b.Name
                                ),
                                ps.Material,
                                ps.Texture
                            )
                        ).Select(p => new KeyValuePair<Guid, PadSeriesReadModel>(p.Id, p))
                    );

                    var polisherTypes = reader.Read<PadSeriesPolisherTypeRow>();
                    foreach (var pt in polisherTypes) {
                        PadSeriesReadModel? s;

                        if (series.TryGetValue(pt.PadSeriesId, out s)) {
                            s.PolisherTypes.Add(pt.PolisherType);
                        }
                    }

                    var sizes = reader.Read<PadSizeRow>();
                    foreach (var size in sizes) {
                        PadSeriesReadModel? s;

                        if (series.TryGetValue(size.PadSeriesId, out s)) {
                            s.Sizes.Add(new PadSizeReadModel(size.Diameter, size.Thickness));
                        }
                    }

                    var reviewCounts = new Dictionary<Guid, int>(reader.Read<(int Count, Guid Id)>().Select(c => new KeyValuePair<Guid, int>(c.Id, c.Count)));
                    var colors = new Dictionary<Guid, PadColorReadModel>();

                    foreach (var raw in reader.Read()) {
                        var color = new PadColorReadModel(
                            raw.id,
                            raw.name,
                            raw.category,
                            raw.image_name != null ? new DataUrlImage(raw.image_name, raw.image_data) : null,
                            new List<PadOptionReadModel>(),
                            raw.cut,
                            raw.finish,
                            new RatingReadModel(raw.stars, reviewCounts[raw.id])
                        );

                        colors.Add(color.Id, color);

                        PadSeriesReadModel? s;

                        if (series.TryGetValue(raw.pad_series_id, out s)) {
                            s!.Colors.Add(color);
                        }
                    }

                    var options = reader.Read<PadOptionRow>();
                    foreach (var opt in options) {
                        PadColorReadModel? color;

                        if (colors.TryGetValue(opt.PadColorId, out color)) {
                            color.Options.Add(new PadOptionReadModel(opt.PadSizeId, opt.PartNumber));
                        }
                    }

                    return series.Values.ToList();
                }
            }
        }
    }
}