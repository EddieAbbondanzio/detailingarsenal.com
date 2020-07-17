using System;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionReadModel : IDataTransferObject {
        public string PlanName { get; }
        public SubscriptionPlanPriceReadModel Price { get; }
        public string Status { get; }
        public DateTime? NextPayment { get; }
        public DateTime TrialStart { get; }
        public DateTime TrialEnd { get; }

        public SubscriptionReadModel(string planName, SubscriptionPlanPriceReadModel price, string status, DateTime? nextPayment, DateTime trialStart, DateTime trialEnd) {
            PlanName = planName;
            Price = price;
            Status = status;
            NextPayment = nextPayment;
            TrialStart = trialStart;
            TrialEnd = trialEnd;
        }
    }
}