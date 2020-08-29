using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "update", Scope = "brands")]
    [Validation(typeof(BrandUpdateValidator))]
    public class UpdateBrandHandler : ActionHandler<BrandUpdateCommand, CommandResult> {
        IBrandRepo repo;
        BrandNameUniqueSpecification unique;

        public UpdateBrandHandler(IBrandRepo repo, BrandNameUniqueSpecification unique) {
            this.repo = repo;
            this.unique = unique;
        }

        public async override Task<CommandResult> Execute(BrandUpdateCommand input, User? user) {
            var brand = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await unique.CheckAndThrow(brand);
            await repo.Update(brand);

            return CommandResult.Success(new {
                Id = brand.Id
            });
        }
    }
}