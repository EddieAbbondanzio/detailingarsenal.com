using System;

namespace DetailingArsenal.Domain.Billing {
    public class Subscription : Entity<Subscription> {
        public Guid CustomerId { get; set; }
        public Guid PlanId { get; set; }
        public string ExternalId { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}