using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Persistence.Shared;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog {
    internal class PadRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public string Name { get; set; } = null!;
        public PadCategoryBitwise Category { get; set; }
        public string? Material { get; set; }
        public string? Texture { get; set; }
        public string? Color { get; set; }
        public bool? HasCenterHole { get; set; }
    }
}