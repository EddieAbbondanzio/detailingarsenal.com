using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class BrandModel {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}