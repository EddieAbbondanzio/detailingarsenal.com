using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public class ServiceNameUniqueSpecification : Specification<Service> {
        public IServiceRepo repo;

        public ServiceNameUniqueSpecification(IServiceRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Service entity) {
            var existingService = await repo.FindByName(entity.Name);

            if (existingService != null && existingService.Id != entity.Id) {
                return new SpecificationResult(false, $"Service name {existingService.Name} is already in use.");
            }

            return new SpecificationResult(true);
        }
    }
}