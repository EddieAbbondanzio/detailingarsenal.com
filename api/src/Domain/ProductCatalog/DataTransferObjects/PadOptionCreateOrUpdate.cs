using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadOptionCreateOrUpdate : IDataTransferObject {
        public Guid? PadSizeId { get; }
        public int? PadSizeIndex { get; }
        public List<PartNumber> PartNumbers { get; }

        [JsonConstructor]
        public PadOptionCreateOrUpdate(Guid? padSizeId, int? padSizeIndex, List<PartNumber>? partNumbers = null) {
            PadSizeId = padSizeId;
            PadSizeIndex = padSizeIndex;
            PartNumbers = partNumbers ?? new();
        }

        public PadOptionCreateOrUpdate(int padSizeIndex, List<PartNumber>? partNumbers = null) {
            PadSizeIndex = padSizeIndex;
            PartNumbers = partNumbers ?? new();
        }

        public PadOptionCreateOrUpdate(Guid padSizeId, List<PartNumber>? partNumbers = null) {
            PadSizeId = padSizeId;
            PartNumbers = partNumbers = new();
        }
    }
}