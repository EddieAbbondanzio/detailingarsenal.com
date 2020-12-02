using System;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadOptionRaw : IDataTransferObject {
        public Guid PadSizeId { get; set; }
        public string? PartNumber { get; set; }
    }
}