using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(ActionHandler<GetAllBrandsQuery, List<BrandReadModel>>))]
    public class GetAllBrandsHandler : ActionHandler<GetAllBrandsQuery, List<BrandReadModel>> {
        IBrandReader reader;

        public GetAllBrandsHandler(IBrandReader reader) {
            this.reader = reader;
        }

        public async override Task<List<BrandReadModel>> Execute(GetAllBrandsQuery input, User? user) {
            var brands = await reader.ReadAll();
            return brands;
        }
    }
}