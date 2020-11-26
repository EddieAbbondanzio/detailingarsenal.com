using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Domain.Common {
    public class RefreshSubscriptionPlansStep : SagaStep {
        ISubscriptionPlanRefresher subscriptionPlanService;

        public RefreshSubscriptionPlansStep(ISubscriptionPlanRefresher subscriptionPlanService) {
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public async override Task Execute(SagaContext context) {
            await subscriptionPlanService.RefreshPlans();
        }
    }
}