using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "delete", Scope = "brands")]
    public class BrandDeleteHandler : ActionHandler<BrandDeleteCommand> {
        IBrandRepo repo;

        public BrandDeleteHandler(IBrandRepo repo) {
            this.repo = repo;
        }

        public async override Task Execute(BrandDeleteCommand input, User? user) {
            var brand = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await repo.Delete(
                brand
            );
        }
    }
}