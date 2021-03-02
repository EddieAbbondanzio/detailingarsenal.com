using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSummaryReader : DatabaseInteractor, IPadSummaryReader {
        public PadSummaryReader(IDatabase database) : base(database) { }

        public async Task<PagedArray<PadSummaryReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                var raws = await conn.QueryAsync(@"
                    select * from pad_summaries;
                ");

                var summaries = raws.Select(r => new PadSummaryReadModel(
                    r.id,
                    r.name,
                    new PadSummarySeriesReadModel(r.pad_series_id, r.pad_series_name),
                    new BrandReadModel(r.brand_id, r.brand_name),
                    ((PadCategoryBitwise)r.category).ToList(),
                    r.material,
                    r.texture,
                    r.has_center_hole,
                    ((PolisherTypeBitwise)r.polisher_types).ToList(),
                    r.cut,
                    r.finish,
                    new PadSummaryRatingReadModel(r.stars, r.review_count)
                ));

                return new PagedArray<PadSummaryReadModel>(new Paging(0, 0, summaries.Count()), summaries.ToArray());
            }
        }
    }
}