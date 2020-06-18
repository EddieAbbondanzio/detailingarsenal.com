using System;

namespace DetailingArsenal.Domain {
    public class Customer : Aggregate<Customer>, IUserEntity {
        public Guid UserId { get; set; }
        public CustomerInfo Info { get; set; } = null!;

        public Customer() { }

        public static Customer Create(Guid userId, CustomerInfo info) {
            return new Customer() {
                Id = Guid.NewGuid(),
                UserId = userId,
                Info = info
            };
        }
    }
}