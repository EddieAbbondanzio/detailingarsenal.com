using System;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionReadModel : IDataTransferObject {
        public string Name { get; }
        public string Status { get; }
        public DateTime TrialStart { get; }
        public DateTime TrialEnd { get; }
        public SubscriptionPlanPriceReadModel Price { get; }


        public SubscriptionReadModel(string name, string status, DateTime trialStart, DateTime trialEnd, SubscriptionPlanPriceReadModel price) {
            Name = name;
            Status = status;
            TrialStart = trialStart;
            TrialEnd = trialEnd;
            Price = price;
        }
    }
}