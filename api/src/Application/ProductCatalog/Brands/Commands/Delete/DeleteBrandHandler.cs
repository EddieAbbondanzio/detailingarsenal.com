using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "delete", Scope = "brands")]
    public class DeleteBrandHandler : ActionHandler<DeleteBrandCommand> {
        IBrandService service;
        IMapper mapper;

        public DeleteBrandHandler(IBrandService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task Execute(DeleteBrandCommand input, User? user) {
            var brand = await service.GetById(input.Id);

            await service.Delete(
                brand, user!
            );
        }
    }
}