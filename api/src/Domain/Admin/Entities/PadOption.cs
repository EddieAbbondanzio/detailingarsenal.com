using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    /// <summary>
    /// Specific pad size and part number combo.
    /// </summary>
    public class PadOption : Entity<PadOption> {
        public Guid PadSizeId { get; }
        public List<PartNumber> PartNumbers { get; }

        public PadOption(Guid? id, Guid padSizeId, List<PartNumber>? partNumbers = null) {
            Id = id ?? Guid.NewGuid();
            PadSizeId = padSizeId;
            PartNumbers = partNumbers ?? new();
        }

        public PadOption(Guid padSizeId, List<PartNumber>? partNumbers = null) {
            Id = Guid.NewGuid();
            PadSizeId = padSizeId;
            PartNumbers = partNumbers ?? new();
        }
    }
}