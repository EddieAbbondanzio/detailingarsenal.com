using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [Authorization(Action = "delete", Scope = "brands")]
    [DependencyInjection]
    public class BrandDeleteHandler : ActionHandler<BrandDeleteCommand> {
        IBrandRepo repo;
        BrandNotInUseSpecification spec;

        public BrandDeleteHandler(IBrandRepo repo, BrandNotInUseSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task Execute(BrandDeleteCommand input, User? user) {
            var brand = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await spec.CheckAndThrow(brand);

            await repo.Delete(
                brand
            );
        }
    }
}