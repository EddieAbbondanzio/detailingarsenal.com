using System.Threading.Tasks;
using DetailingArsenal.Domain.Calendar;

namespace DetailingArsenal.Domain.Clients {
    [DependencyInjection]
    public class ClientHasNoAppointmentsSpecification : Specification<Client> {
        private IAppointmentRepo repo;

        public ClientHasNoAppointmentsSpecification(IAppointmentRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Client entity) {
            int appCount = await repo.CountForClient(entity);

            if (appCount == 0) {
                return Satisfied();
            }

            return new SpecificationResult(false, $"Client has {appCount} appointments. Cannot delete.");
        }
    }
}