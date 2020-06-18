using System;

namespace DetailingArsenal.Domain {
    public class Customer : Aggregate<Customer>, IUserEntity {
        public Guid UserId { get; set; }
        public ExternalCustomer External { get; set; } = null!;

        public Customer() { }

        public static Customer Create(Guid userId, ExternalCustomer external) {
            return new Customer() {
                Id = Guid.NewGuid(),
                UserId = userId,
                External = external
            };
        }
    }
}