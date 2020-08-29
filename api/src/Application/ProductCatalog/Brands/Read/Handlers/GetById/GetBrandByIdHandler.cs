using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetBrandByIdHandler : ActionHandler<GetBrandByIdQuery, BrandReadModel?> {
        IBrandReader reader;

        public GetBrandByIdHandler(IBrandReader reader) {
            this.reader = reader;
        }

        public async override Task<BrandReadModel?> Execute(GetBrandByIdQuery input, User? user) {
            var brand = await reader.ReadById(input.Id);
            return brand;
        }
    }
}