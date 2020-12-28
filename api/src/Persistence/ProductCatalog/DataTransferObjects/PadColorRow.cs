using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PadColorRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public Guid? ImageId { get; set; }
    }
}