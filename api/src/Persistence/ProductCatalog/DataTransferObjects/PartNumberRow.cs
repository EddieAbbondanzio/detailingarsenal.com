using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PartNumberRow : IDataTransferObject {
        public Guid Id { get; set; } // Persistence use only
        public string Value { get; set; } = null!;
        public string? Notes { get; set; }
    }
}