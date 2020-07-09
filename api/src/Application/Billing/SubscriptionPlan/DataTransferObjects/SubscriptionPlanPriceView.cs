using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Billing {
    public class SubscriptionPlanPriceView : IDataTransferObject {
        public decimal Amount { get; set; }
        public string Interval { get; set; } = null!;
        public string BillingId { get; set; } = null!;
    }
}