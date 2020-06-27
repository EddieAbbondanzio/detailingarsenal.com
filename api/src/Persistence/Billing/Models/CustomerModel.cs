using System;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerModel {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ExternalId { get; set; } = null!;
    }
}