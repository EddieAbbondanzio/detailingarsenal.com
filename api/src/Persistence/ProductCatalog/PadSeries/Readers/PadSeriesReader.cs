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

        public async Task<List<PadSeriesReadModel>> ReadAll() {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select * from pad_series ps join brands b on ps.brand_id = b.id; 
                    select * from pads;"
            )) {
                var series = reader.Read<PadSeriesModel, BrandModel, PadSeriesReadModel>(
                    (ps, b) => new PadSeriesReadModel(
                        ps.Id, ps.Name, new BrandReadModel(b.Id, b.Name)
                    )
                );

                var pads = reader.Read<PadModel>();

                // Gotta get that O(1) lookup time.
                var lookup = new Dictionary<Guid, PadSeriesReadModel>(series.Select(s => new KeyValuePair<Guid, PadSeriesReadModel>(s.Id, s)));

                foreach (PadModel pad in pads) {
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