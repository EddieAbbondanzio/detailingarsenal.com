using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    [DependencyInjection]
    public class VehicleCategoryNotInUseSpecification : Specification<VehicleCategory> {
        public IVehicleCategoryRepo repo;

        public VehicleCategoryNotInUseSpecification(IVehicleCategoryRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(VehicleCategory entity) {
            var inUse = await repo.IsInUse(entity);

            if (!inUse) {
                return Satisfied();
            }

            return new SpecificationResult(false, "Vehicle category is in use for appointments and/or service configurations");
        }
    }
}