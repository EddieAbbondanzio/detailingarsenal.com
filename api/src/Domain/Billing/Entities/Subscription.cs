using System;

namespace DetailingArsenal.Domain {
    public class Subscription : Aggregate<Subscription>, IUserEntity {
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
        public string ExternalId { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}