using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Domain.Common {
    public class RefreshSubscriptionPlansStep : SagaStep {
        ISubscriptionPlanService subscriptionPlanService;

        public RefreshSubscriptionPlansStep(ISubscriptionPlanService subscriptionPlanService) {
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public async override Task Execute() {
            await subscriptionPlanService.RefreshPlans();
        }
    }
}