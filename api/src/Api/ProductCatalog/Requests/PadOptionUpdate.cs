using System;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadOptionUpdate : IDataTransferObject {
        public Guid PadSizeId { get; set; }
        public string? PartNumber { get; set; }
    }
}