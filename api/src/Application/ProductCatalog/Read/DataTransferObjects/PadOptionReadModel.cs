using System;
using System.Collections.Generic;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadOptionReadModel : IAction {
        public Guid PadSizeId { get; set; }
        public List<PartNumberReadModel> PartNumbers { get; set; }

        public PadOptionReadModel(Guid padSizeId, List<PartNumberReadModel>? partNumbers = null) {
            PadSizeId = padSizeId;
            PartNumbers = partNumbers ?? new();
        }
    }
}