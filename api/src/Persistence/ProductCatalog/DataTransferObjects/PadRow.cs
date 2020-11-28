using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string Texture { get; set; } = null!;
        public string? ImageName { get; set; }
        public byte[]? ImageData { get; set; }
    }
}