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
                        select * from pads p where p.pad_series_id = @Id;
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

                    var pads = reader.Read<PadRow>().Select(p => new PadReadModel(
                       p.Id,
                       p.Name,
                       p.Category,
                       0, // pull from reviews
                       0, // pull from reviews 
                       p.Material,
                       p.Texture
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

                    series.Pads = pads;
                    series.Sizes = sizes;
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

                    throw new NotImplementedException();


                    // foreach (PadRow pad in pads) {
                    //     PadSeriesReadModel? s = null;

                    //     if (lookup.TryGetValue(pad.PadSeriesId, out s)) {
                    //         var image = pad.ImageName != null ? new DataUrlImage(pad.ImageName, pad.ImageData!) : null;
                    //         s.Pads.Add(new PadReadModel(pad.Id, pad.Category, pad.Name, image));
                    //     }
                    // }

                    // return series.ToList();
                }
            }
        }
    }
}