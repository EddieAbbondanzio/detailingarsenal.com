using System;

namespace DetailingArsenal.Domain {
    public class Customer : Entity<Customer>, IUserEntity {
        public Guid UserId { get; set; }
        public CustomerInfo Info { get; set; } = null!;
    }
}