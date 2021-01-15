using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadOptionCreateOrUpdate : IDataTransferObject {
        public Guid? PadSizeId { get; set; }
        public int? PadSizeIndex { get; set; }
        public string? PartNumber { get; set; }

        public PadOptionCreateOrUpdate() {
            // used by asp.net 
        }

        public PadOptionCreateOrUpdate(int padSizeIndex, string? partNumber = null) {
            PadSizeIndex = padSizeIndex;
            PartNumber = partNumber;
        }

        public PadOptionCreateOrUpdate(Guid padSizeId, string? partNumber) {
            PadSizeId = padSizeId;
            PartNumber = partNumber;
        }
    }
}