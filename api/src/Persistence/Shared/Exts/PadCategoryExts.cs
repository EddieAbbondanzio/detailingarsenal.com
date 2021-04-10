using System.Collections.Generic;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {
    internal static class PadCategoryExts {
        public static PadCategoryBitwise Flatten(this List<PadCategory> category) {
            var bitwise = PadCategoryBitwise.None;

            for (int i = 0; i < category.Count; i++) {
                switch (category[i]) {
                    case PadCategory.Cutting:
                        bitwise |= PadCategoryBitwise.Cutting;
                        break;
                    case PadCategory.Polishing:
                        bitwise |= PadCategoryBitwise.Polishing;
                        break;
                    case PadCategory.Finishing:
                        bitwise |= PadCategoryBitwise.Finishing;
                        break;
                }
            }

            return bitwise;
        }
    }
}