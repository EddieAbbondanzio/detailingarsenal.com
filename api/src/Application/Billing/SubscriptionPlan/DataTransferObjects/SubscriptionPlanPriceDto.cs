using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Billing {
    public class SubscriptionPlanPriceDto : IDataTransferObject {
        public string ExternalId { get; set; } = null!;
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
    }
}