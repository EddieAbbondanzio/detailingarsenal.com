using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSummaryReader : DatabaseInteractor, IPadSummaryReader {
        public PadSummaryReader(IDatabase database) : base(database) { }

        public async Task<PadSummaryReadModel?> Read(Guid id) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync<PadsViewRow>(@"
                    select * from pads_view where id = @Id;
                ", new { Id = id });

                var summary = new PadSummaryReadModel(
                    r.Id,
                    r.Name,
                    new PadSummarySeriesReadModel(r.PadSeriesId, r.PadSeriesName),
                    new BrandReadModel(r.BrandId, r.BrandName),
                    ((PadCategoryBitwise)r.Category).ToList(),
                    PadMaterialUtils.Parse(r.Material),
                    PadTextureUtils.Parse(r.Texture),
                    r.HasCenterHole,
                    ((PolisherTypeBitwise)r.PolisherTypes).ToList(),
                    r.Cut,
                    r.Finish,
                    new PadSummaryRatingReadModel(r.Stars, r.ReviewCount)
                );

                return summary;
            }
        }

        public async Task<PagedCollection<PadSummaryReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                var raws = await conn.QueryAsync<PadsViewRow>(@"
                    select * from pads_view;
                ");

                var summaries = raws.Select(r => new PadSummaryReadModel(
                    r.Id,
                    r.Name,
                    new PadSummarySeriesReadModel(r.PadSeriesId, r.PadSeriesName),
                    new BrandReadModel(r.BrandId, r.BrandName),
                    ((PadCategoryBitwise)r.Category).ToList(),
                    PadMaterialUtils.Parse(r.Material),
                    PadTextureUtils.Parse(r.Texture),
                    r.HasCenterHole,
                    ((PolisherTypeBitwise)r.PolisherTypes).ToList(),
                    r.Cut,
                    r.Finish,
                    new PadSummaryRatingReadModel(r.Stars, r.ReviewCount)
                ));

                return new PagedCollection<PadSummaryReadModel>(new Paging(0, 20, summaries.Count()), summaries.ToArray());
            }
        }
    }
}