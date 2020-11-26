using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class SubscriptionPlanView : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<SubscriptionPlanPriceView> Prices { get; set; } = null!;
    }
}