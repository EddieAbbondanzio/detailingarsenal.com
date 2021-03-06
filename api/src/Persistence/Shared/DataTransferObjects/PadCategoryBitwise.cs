using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {
    [Flags]
    internal enum PadCategoryBitwise {
        None = 0,
        Cutting = 1,
        Polishing = 1 << 1,
        Finishing = 1 << 2
    }

    internal static class PadCategoryBitwiseExts {
        public static List<PadCategory> ToList(this PadCategoryBitwise bitwise) {
            List<PadCategory> l = new();

            if (bitwise.HasFlag(PadCategoryBitwise.Cutting)) {
                l.Add(PadCategory.Cutting);
            }

            if (bitwise.HasFlag(PadCategoryBitwise.Polishing)) {
                l.Add(PadCategory.Polishing);
            }

            if (bitwise.HasFlag(PadCategoryBitwise.Finishing)) {
                l.Add(PadCategory.Finishing);
            }

            return l;
        }
    }
}