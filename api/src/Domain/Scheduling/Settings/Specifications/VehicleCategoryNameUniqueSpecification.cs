using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    [DependencyInjection]
    public class VehicleCategoryNameUniqueSpecification : Specification<VehicleCategory> {
        public IVehicleCategoryRepo repo;

        public VehicleCategoryNameUniqueSpecification(IVehicleCategoryRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(VehicleCategory entity) {
            var existingVC = await repo.FindByName(entity.Name);

            if (existingVC != null && existingVC.Id != entity.Id) {
                return new SpecificationResult(false, $"Vehicle category name {existingVC.Name} is already in use.");
            }

            return new SpecificationResult(true);
        }
    }
}