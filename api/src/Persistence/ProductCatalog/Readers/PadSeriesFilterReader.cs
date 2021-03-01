using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesFilterReader : DatabaseInteractor, IPadSeriesFilterReader {
        public PadSeriesFilterReader(IDatabase database) : base(database) { }

        public async Task<PadSeriesFilterReadModel> Read() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(@"
                    select id, name from brands;
                    select ps.id, ps.name, b.name as brand_name from pad_series ps join brands b on ps.brand_id = b.id;
                ")) {
                    var brands = reader.Read<PadSeriesFilterBrandReadModel>();
                    var series = reader.Read().Select(s => new PadSeriesFilterSeriesReadModel(s.id, s.name, s.brand_name)); //Dapper isn't find ctor.

                    return new PadSeriesFilterReadModel(brands, series);
                }
            }
        }
    }
}