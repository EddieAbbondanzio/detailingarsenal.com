using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Common {
    public class RefreshSubscriptionPlans : SagaStep {
        ISubscriptionPlanService subscriptionPlanService;

        public RefreshSubscriptionPlans(ISubscriptionPlanService subscriptionPlanService) {
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public async override Task Execute() {
            await subscriptionPlanService.RefreshPlans();
        }

        public override Task Compensate() {
            throw new System.NotImplementedException();
        }
    }
}