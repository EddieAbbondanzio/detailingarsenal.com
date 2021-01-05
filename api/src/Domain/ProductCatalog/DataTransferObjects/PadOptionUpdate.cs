using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadOptionUpdate : IDataTransferObject {
        public Guid PadSizeId { get; set; }
        public string? PartNumber { get; set; }
    }
}