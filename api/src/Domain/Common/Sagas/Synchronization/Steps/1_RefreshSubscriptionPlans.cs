using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Domain.Common {
    public class RefreshSubscriptionPlansStep : SagaStep {
        ISubscriptionPlanService subscriptionPlanService;

        public RefreshSubscriptionPlansStep(ISubscriptionPlanService subscriptionPlanService) {
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public async override Task Execute(SagaContext context) {
            await subscriptionPlanService.RefreshPlans();
        }
    }
}