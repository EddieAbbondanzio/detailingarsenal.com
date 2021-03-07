using System;
using System.Dynamic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [Authorization(Action = "create", Scope = "brands")]
    [Validation(typeof(BrandCreateValidator))]
    [DependencyInjection(RegisterAs = typeof(ActionHandler<BrandCreateCommand, Guid>))]
    public class BrandCreateHandler : ActionHandler<BrandCreateCommand, Guid> {
        IBrandRepo repo;
        BrandNameUniqueSpecification unique;

        public BrandCreateHandler(IBrandRepo repo, BrandNameUniqueSpecification unique) {
            this.repo = repo;
            this.unique = unique;
        }

        public async override Task<Guid> Execute(BrandCreateCommand input, User? user) {
            var brand = new Brand(input.Name);

            await unique.CheckAndThrow(brand);
            await repo.Add(brand);

            return brand.Id;
        }
    }
}