using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Billing {
    public class SubscriptionPlanPriceView : IDataTransferObject {
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
    }
}