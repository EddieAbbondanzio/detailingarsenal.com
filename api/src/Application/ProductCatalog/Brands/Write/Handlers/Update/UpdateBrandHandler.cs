using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "update", Scope = "brands")]
    [Validation(typeof(UpdateBrandValidator))]
    public class UpdateBrandHandler : ActionHandler<UpdateBrandCommand, BrandReadModel> {
        IBrandService service;
        IMapper mapper;

        public UpdateBrandHandler(IBrandService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<BrandReadModel> Execute(UpdateBrandCommand input, User? user) {
            var brand = await service.GetById(input.Id);

            brand = await service.Update(brand, new BrandUpdate(input.Name), user!);

            return mapper.Map<Brand, BrandReadModel>(brand);
        }
    }
}