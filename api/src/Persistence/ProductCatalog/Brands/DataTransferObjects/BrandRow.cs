using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class BrandRow {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}