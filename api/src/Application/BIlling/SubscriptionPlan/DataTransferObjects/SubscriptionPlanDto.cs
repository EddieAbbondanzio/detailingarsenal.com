using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class SubscriptionPlanDto : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string ExternalId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<SubscriptionPlanPriceDto> Prices { get; set; } = null!;
    }
}