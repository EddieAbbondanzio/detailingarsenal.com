using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class BrandRow {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}