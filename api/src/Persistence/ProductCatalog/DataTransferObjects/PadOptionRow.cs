using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PadOptionRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadColorId { get; set; }
        public Guid PadSizeId { get; set; }
        public string? PartNumber { get; set; }
    }
}