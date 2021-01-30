using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// Specific pad size and part number combo.
    /// </summary>
    public class PadOption : ValueObject<PadOption> {
        public Guid PadSizeId { get; }
        public List<PartNumber> PartNumbers { get; }

        public PadOption(Guid padSizeId, List<PartNumber>? partNumbers = null) {
            PadSizeId = padSizeId;
            PartNumbers = partNumbers ?? new();
        }
    }
}