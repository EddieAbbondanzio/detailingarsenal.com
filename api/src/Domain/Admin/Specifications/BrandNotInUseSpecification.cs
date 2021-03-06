using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public class BrandNotInUseSpecification : Specification<Brand> {
        IBrandRepo repo;

        public BrandNotInUseSpecification(IBrandRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Brand entity) {
            var inUse = await repo.IsBrandInUse(entity);
            if (inUse) {
                return new SpecificationResult(false, "Brand is in use.");
            } else {
                return new SpecificationResult(true);
            }
        }
    }
}