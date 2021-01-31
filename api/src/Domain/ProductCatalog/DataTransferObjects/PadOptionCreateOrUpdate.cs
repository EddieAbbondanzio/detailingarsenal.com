using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadOptionCreateOrUpdate : IDataTransferObject {
        public int PadSizeIndex { get; }
        public List<PartNumber> PartNumbers { get; }

        public PadOptionCreateOrUpdate(int padSizeIndex, List<PartNumber>? partNumbers = null) {
            PadSizeIndex = padSizeIndex;
            PartNumbers = partNumbers ?? new();
        }
    }
}