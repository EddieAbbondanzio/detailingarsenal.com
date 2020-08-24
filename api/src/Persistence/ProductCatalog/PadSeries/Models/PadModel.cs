using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public PadCategory Category { get; set; }
        public string Name { get; set; } = null!;
        public byte[]? Image { get; set; }
    }
}