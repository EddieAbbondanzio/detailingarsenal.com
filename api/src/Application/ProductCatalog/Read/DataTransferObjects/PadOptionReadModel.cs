using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadOptionReadModel : IDataTransferObject {
        public Guid? Id { get; }
        public Guid PadSizeId { get; }
        public List<PartNumberReadModel> PartNumbers { get; }

        public PadOptionReadModel(Guid id, Guid padSizeId, List<PartNumberReadModel>? partNumbers = null) {
            Id = id;
            PadSizeId = padSizeId;
            PartNumbers = partNumbers ?? new();
        }
    }
}