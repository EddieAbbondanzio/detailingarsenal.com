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
                    select id, name from pad_series;
                    select id, name from brands;
                ")) {
                    var series = reader.Read<PadSeriesFilterOptionReadModel>();
                    var brands = reader.Read<PadSeriesFilterOptionReadModel>();

                    return new PadSeriesFilterReadModel(series, brands);
                }
            }
        }
    }
}