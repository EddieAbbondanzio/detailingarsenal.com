using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(IPadFilterReader))]
    public class PadFilterReader : DatabaseInteractor, IPadFilterReader {
        public PadFilterReader(IDatabase database) : base(database) { }

        public async Task<PadFilterReadModel> Read() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(@"
                    select b.id, b.name from pads p 
                        join pad_series ps on p.pad_series_id = ps.id 
                        join brands b on ps.brand_id = b.id 
                        group by b.id 
                        order by b.name;
                    select ps.id, ps.name, b.name as brand_name from pad_series ps 
                        join brands b on ps.brand_id = b.id
                        order by ps.name;
                ")) {
                    var brands = reader.Read<PadFilterBrandReadModel>();
                    var series = reader.Read().Select(s => new PadFilterSeriesReadModel(s.id, s.name, s.brand_name)); //Dapper isn't find ctor.

                    return new PadFilterReadModel(brands, series);
                }
            }
        }
    }
}