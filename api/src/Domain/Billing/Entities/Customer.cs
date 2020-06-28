using System;

namespace DetailingArsenal.Domain.Billing {
    public class Customer : Aggregate<Customer>, IUserEntity {
        public Guid UserId { get; set; }
        public string ExternalId { get; set; } = null!;
        public Subscription Subscription { get; set; } = null!;
    }
}