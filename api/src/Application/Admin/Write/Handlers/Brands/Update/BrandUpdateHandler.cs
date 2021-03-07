using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [Authorization(Action = "update", Scope = "brands")]
    [Validation(typeof(BrandUpdateValidator))]
    [DependencyInjection]
    public class BrandUpdateHandler : ActionHandler<BrandUpdateCommand> {
        IBrandRepo repo;
        BrandNameUniqueSpecification unique;

        public BrandUpdateHandler(IBrandRepo repo, BrandNameUniqueSpecification unique) {
            this.repo = repo;
            this.unique = unique;
        }

        public async override Task Execute(BrandUpdateCommand input, User? user = null) {
            var brand = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            brand.Name = input.Name;

            await unique.CheckAndThrow(brand);
            await repo.Update(brand);
        }
    }
}