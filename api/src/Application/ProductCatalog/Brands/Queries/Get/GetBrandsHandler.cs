using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetBrandsHandler : ActionHandler<GetBrandsQuery, List<BrandReadModel>> {
        IBrandReader reader;

        public GetBrandsHandler(IBrandReader reader) {
            this.reader = reader;
        }

        public async override Task<List<BrandReadModel>> Execute(GetBrandsQuery input, User? user) {
            var brands = await reader.ReadAll();
            return brands;
        }
    }
}