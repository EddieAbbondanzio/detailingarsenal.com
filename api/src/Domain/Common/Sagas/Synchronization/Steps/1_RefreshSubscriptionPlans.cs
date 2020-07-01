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

#pragma warning disable 1998
        public async override Task Compensate() {
            // throw new System.NotImplementedException();
        }
#pragma warning restore 1998
    }
}