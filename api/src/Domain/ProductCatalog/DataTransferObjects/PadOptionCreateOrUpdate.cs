using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadOptionCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public int PadSizeIndex { get; }
        public List<PartNumber> PartNumbers { get; }

        [JsonConstructor]
        public PadOptionCreateOrUpdate(Guid? id, int padSizeIndex, List<PartNumber>? partNumbers = null) {
            PadSizeIndex = padSizeIndex;
            PartNumbers = partNumbers ?? new();
        }
    }
}