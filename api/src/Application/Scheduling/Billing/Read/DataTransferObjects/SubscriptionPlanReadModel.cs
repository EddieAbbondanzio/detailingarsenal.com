using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class SubscriptionPlanReadModel : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<SubscriptionPlanPriceReadModel> Prices { get; set; } = null!;
    }
}