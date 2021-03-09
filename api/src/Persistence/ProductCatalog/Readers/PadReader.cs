using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Persistence.Shared;

namespace DetailingArsenal.Persistence.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(IPadReader))]
    public class PadReader : DatabaseInteractor, IPadReader {
        public PadReader(IDatabase database) : base(database) { }

        public async Task<PadReadModel?> Read(Guid id) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync<PadsViewRow>(@"
                    select * from pads_view where id = @Id;
                ", new { Id = id });

                var summary = new PadReadModel(
                    r.Id,
                    r.Name,
                    new(r.PadSeriesId, r.PadSeriesName),
                    new(r.BrandId, r.BrandName),
                    ((PadCategoryBitwise)r.Category).ToList(),
                    PadMaterialUtils.Parse(r.Material),
                    PadTextureUtils.Parse(r.Texture),
                    r.HasCenterHole,
                    ((PolisherTypeBitwise)r.PolisherTypes).ToList(),
                    r.Cut,
                    r.Finish,
                    new RatingReadModel(r.Stars, r.ReviewCount)
                );

                return summary;
            }
        }

        public async Task<PagedCollection<PadReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                var raws = await conn.QueryAsync<PadsViewRow>(@"
                    select * from pads_view;
                ");

                var summaries = raws.Select(r => new PadReadModel(
                    r.Id,
                    r.Name,
                    new PadSeriesReadModel(r.PadSeriesId, r.PadSeriesName),
                    new PadBrandReadModel(r.BrandId, r.BrandName),
                    ((PadCategoryBitwise)r.Category).ToList(),
                    PadMaterialUtils.Parse(r.Material),
                    PadTextureUtils.Parse(r.Texture),
                    r.HasCenterHole,
                    ((PolisherTypeBitwise)r.PolisherTypes).ToList(),
                    r.Cut,
                    r.Finish,
                    new RatingReadModel(r.Stars, r.ReviewCount)
                ));

                return new PagedCollection<PadReadModel>(new Paging(0, 20, summaries.Count()), summaries.ToArray());
            }
        }
    }
}