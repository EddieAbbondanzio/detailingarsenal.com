using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PadRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string? Material { get; set; }
        public string? Texture { get; set; }
        public string? Color { get; set; }
        public Guid? ImageId { get; set; }
    }
}