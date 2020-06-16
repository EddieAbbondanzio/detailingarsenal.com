using System;

namespace DetailingArsenal.Domain {
    public class Customer : Aggregate<Customer>, IUserEntity {
        public Guid UserId { get; set; }
        public CustomerInfo Info { get; set; } = null!;
    }
}