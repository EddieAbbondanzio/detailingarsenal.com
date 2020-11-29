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
                    @"
                        select * from pad_series ps join brands b on ps.brand_id = b.id where ps.id = @Id; 
                        select * from pad_series_sizes where pad_series_id = @Id;
                        select p.*, avg(r.cut) as cut, avg(r.finish) as finish from pads p 
                            left join reviews r on p.id = r.pad_id 
                            where pad_series_id = @Id group by p.id;
                        select ppt.* from pad_polisher_types ppt join pads p on ppt.pad_id = p.id where p.pad_series_id = @Id;
                    ",
                    new { Id = id }
                )) {
                    var series = reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                        (ps, b) => new PadSeriesReadModel(
                            ps.Id, ps.Name, new BrandReadModel(b.Id, b.Name)
                        )
                    ).ElementAt(0);

                    if (series == null) {
                        return null;
                    }

                    var sizes = reader.Read<PadSeriesSizeRow>().Select(pss => new PadSeriesSizeReadModel(
                        pss.Diameter,
                        pss.Thickness,
                        pss.PartNumber
                    )).ToList();

                    var pads = reader.Read().Select(p => new PadReadModel(
                       p.id,
                       p.name,
                       p.category,
                       p.cut,   // THIS MIGHT BREAK!
                       p.finish,
                       p.material,
                       p.texture
                    )).ToList();

                    var padLookup = pads.ToDictionary(p => p.Id, p => p);

                    // Assign each pad their polisher types
                    var padPolisherTypes = reader.Read<PadPolisherTypeRow>();
                    foreach (var padPolisherType in padPolisherTypes) {
                        PadReadModel? pad;

                        if (padLookup.TryGetValue(padPolisherType.PadId, out pad)) {
                            pad.PolisherTypes.Add(padPolisherType.PolisherType);
                        }
                    }

                    series.Pads.AddRange(pads);
                    series.Sizes.AddRange(sizes);
                    return series;
                }
            }
        }

        public async Task<List<PadSeriesReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"
                        select * from pad_series ps join brands b on ps.brand_id = b.id; 
                        select * from pad_series_sizes;
                        select p.*, avg(r.cut) as cut, avg(r.finish) as finish from pads p 
                            left join reviews r on p.id = r.pad_id group by p.id;
                        select * from pad_polisher_types;
                    "
                )) {
                    var series = new Dictionary<Guid, PadSeriesReadModel>(reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                        (ps, b) => new PadSeriesReadModel(
                            ps.Id, ps.Name, new BrandReadModel(b.Id, b.Name)
                        )).Select(s => new KeyValuePair<Guid, PadSeriesReadModel>(s.Id, s))
                    );

                    // Stop if we didn't find anything.
                    if (series.Count() == 0) {
                        return new List<PadSeriesReadModel>();
                    }

                    var sizes = reader.Read<PadSeriesSizeRow>();
                    foreach (var size in sizes) {
                        PadSeriesReadModel? ps;

                        if (series.TryGetValue(size.PadSeriesId, out ps)) {
                            ps.Sizes.Add(new PadSeriesSizeReadModel(size.Diameter, size.Thickness, size.PartNumber));
                        }
                    }

                    var pads = reader.Read();
                    var padDict = new Dictionary<Guid, PadReadModel>();
                    foreach (var p in pads) {
                        PadSeriesReadModel? ps;

                        if (series.TryGetValue(p.pad_series_id, out ps)) {
                            var maybe = (IDictionary<string, object>)p;
                            int? cut = null;
                            int? finish = null;

                            if (maybe.ContainsKey("cut")) {
                                cut = p.cut;
                            }

                            if (maybe.ContainsKey("finish")) {
                                finish = p.finish;
                            }


                            var pad = new PadReadModel(
                                p.id,
                                p.name,
                                p.category,
                                cut,
                                finish,
                                p.material,
                                p.texture
                            );

                            padDict.Add(pad.Id, pad);
                            ps!.Pads.Add(pad);

                        }
                    }

                    var polisherTypes = reader.Read<PadPolisherTypeRow>();
                    foreach (var polisherType in polisherTypes) {
                        PadReadModel? pad;

                        if (padDict.TryGetValue(polisherType.PadId, out pad)) {
                            pad.PolisherTypes.Add(polisherType.PolisherType);
                        }
                    }

                    return series.Values.ToList();
                }
            }
        }
    }
}