using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class BrandReader : DatabaseInteractor, IBrandReader {
        public BrandReader(IDatabase database) : base(database) { }

        public async Task<List<BrandReadModel>> ReadAll() {
            var brands = await Connection.QueryAsync<BrandReadModel>(
                @"select id, name from brands;"
            );

            return brands.ToList();
        }
    }
}