using System;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public class Brand : Entity<Brand> {
        public const int NameMaxLength = 32;

        public string Name { get; set; } = null!;

        public Brand(string name) {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Brand(Guid id, string name) {
            Id = id;
            Name = name;
        }
    }
}