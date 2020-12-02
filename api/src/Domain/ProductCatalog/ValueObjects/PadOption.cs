using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// Specific pad size and part number combo.
    /// </summary>
    public class PadOption : ValueObject<PadOption> {
        public Guid PadSizeId { get; }
        public string? PartNumber { get; }

        public PadOption(Guid padSizeId, string? partNumber = null) {
            PadSizeId = padSizeId;
            PartNumber = partNumber;
        }
    }
}