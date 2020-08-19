using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class BrandNameUniqueSpecification : Specification<Brand> {
        public IBrandRepo repo;

        public BrandNameUniqueSpecification(IBrandRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Brand entity) {
            var existingBrand = await repo.FindByName(entity.Name);

            if (existingBrand != null && existingBrand.Id != entity.Id) {
                return new SpecificationResult(false, $"Brand name {existingBrand.Name} is already in use.");
            }

            return new SpecificationResult(true);
        }
    }
}