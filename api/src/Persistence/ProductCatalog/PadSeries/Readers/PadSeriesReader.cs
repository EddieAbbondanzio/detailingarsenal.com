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
                    @"select * from pad_series ps join brands b on ps.brand_id = b.id where ps.id = @Id; 
                    select * from pads p where p.pad_series_id = @Id;"
                , new { Id = id })) {
                    var series = reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                        (ps, b) => new PadSeriesReadModel(
                            ps.Id, ps.Name, new BrandReadModel(b.Id, b.Name)
                        )
                    ).ElementAt(0);

                    if (series == null) {
                        return null;
                    }

                    series.Pads = reader.Read<PadRow>().Select(p => new PadReadModel(
                        p.Id, p.Category, p.Name, p.ImageName != null ? new DataUrlImage(p.ImageName, p.ImageData!) : null
                    )).ToList();

                    return series;
                }
            }
        }

        public async Task<List<PadSeriesReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select * from pad_series ps join brands b on ps.brand_id = b.id; 
                    select * from pads;"
                )) {
                    var series = reader.Read<PadSeriesRow, BrandRow, PadSeriesReadModel>(
                        (ps, b) => new PadSeriesReadModel(
                            ps.Id, ps.Name, new BrandReadModel(b.Id, b.Name)
                        )
                    );

                    var pads = reader.Read<PadRow>();

                    // Gotta get that O(1) lookup time.
                    var lookup = new Dictionary<Guid, PadSeriesReadModel>(series.Select(s => new KeyValuePair<Guid, PadSeriesReadModel>(s.Id, s)));

                    foreach (PadRow pad in pads) {
                        PadSeriesReadModel? s = null;

                        if (lookup.TryGetValue(pad.PadSeriesId, out s)) {
                            var image = pad.ImageName != null ? new DataUrlImage(pad.ImageName, pad.ImageData!) : null;
                            s.Pads.Add(new PadReadModel(pad.Id, pad.Category, pad.Name, image));
                        }
                    }

                    return series.ToList();
                }
            }
        }
    }
}