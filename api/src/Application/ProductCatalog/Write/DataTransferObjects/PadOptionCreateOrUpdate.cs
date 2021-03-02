using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Application.ProductCatalog {
    public class PadOptionCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public int PadSizeIndex { get; }
        public List<PartNumberCreateOrUpdate> PartNumbers { get; }

        [JsonConstructor]
        public PadOptionCreateOrUpdate(Guid? id, int padSizeIndex, List<PartNumberCreateOrUpdate>? partNumbers = null) {
            Id = id;
            PadSizeIndex = padSizeIndex;
            PartNumbers = partNumbers ?? new();
        }
    }
}