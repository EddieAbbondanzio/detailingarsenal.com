using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class RefreshSubscriptionPlansOnStartup : IBusEventHandler<StartupEvent> {
        private SubscriptionPlanService service;

        public RefreshSubscriptionPlansOnStartup(SubscriptionPlanService service) {
            this.service = service;
        }

        public async Task Handle(StartupEvent busEvent) {
            await service.RefreshPlans();
        }
    }
}