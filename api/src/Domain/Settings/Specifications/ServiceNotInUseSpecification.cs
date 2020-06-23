using System.Threading.Tasks;
using DetailingArsenal.Domain.Calendar;

namespace DetailingArsenal.Domain.Settings {
    public class ServiceNotInUseSpecification : Specification<Service> {
        private IAppointmentRepo repo;

        public ServiceNotInUseSpecification(IAppointmentRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Service entity) {
            int usageCount = await repo.CountForService(entity);

            if (usageCount == 0) {
                return Satisfied();
            }

            return new SpecificationResult(false, $"{usageCount} appointments exist for this service.");
        }
    }
}