using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "brands")]
    [Validation(typeof(BrandCreateValidator))]
    public class BrandCreateHandler : ActionHandler<BrandCreateCommand, CommandResult> {
        IBrandRepo repo;
        BrandNameUniqueSpecification unique;

        public BrandCreateHandler(IBrandRepo repo, BrandNameUniqueSpecification unique) {
            this.repo = repo;
            this.unique = unique;
        }

        public async override Task<CommandResult> Execute(BrandCreateCommand input, User? user) {
            var brand = new Brand(input.Name);

            await unique.CheckAndThrow(brand);
            await repo.Add(brand);

            var res = CommandResult.Success(new {
                Id = brand.Id
            });

            return res;
        }
    }
}