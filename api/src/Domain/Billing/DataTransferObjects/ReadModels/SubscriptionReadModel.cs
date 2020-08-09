using System;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionReadModel : IDataTransferObject {
        public string PlanName { get; }
        public SubscriptionPlanPriceReadModel Price { get; }
        public string Status { get; }
        public PeriodReadModel TrialPeriod { get; }
        public PeriodReadModel Period { get; }
        public bool CancellingAtPeriodEnd { get; }

        public SubscriptionReadModel(string planName, SubscriptionPlanPriceReadModel price, string status, PeriodReadModel trialPeriod, PeriodReadModel period, bool cancellingAtPeriodEnd) {
            PlanName = planName;
            Price = price;
            Status = status;
            TrialPeriod = trialPeriod;
            Period = period;
            CancellingAtPeriodEnd = cancellingAtPeriodEnd;
        }
    }
}