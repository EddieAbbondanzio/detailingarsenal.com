using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "brands")]
    [Validation(typeof(CreateBrandValidator))]
    public class CreateBrandHandler : ActionHandler<CreateBrandCommand, BrandReadModel> {
        IBrandService service;
        IMapper mapper;

        public CreateBrandHandler(IBrandService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<BrandReadModel> Execute(CreateBrandCommand input, User? user) {
            var brand = await service.Create(new BrandCreate(
                input.Name
            ), user!);

            return mapper.Map<Brand, BrandReadModel>(brand);
        }
    }
}