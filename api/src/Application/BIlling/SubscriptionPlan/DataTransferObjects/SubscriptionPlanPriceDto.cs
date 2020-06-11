using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class SubscriptionPlanPriceDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } = null!;
        public decimal Price { get; set; }
        public string Interval { get; set; } = null!;
    }
}