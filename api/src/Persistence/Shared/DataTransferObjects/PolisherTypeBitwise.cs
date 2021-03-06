using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {
    [Flags]
    internal enum PolisherTypeBitwise {
        None = 0,
        DualAction = 1 << 0,
        LongThrow = 1 << 1,
        ForcedRotation = 1 << 2,
        Rotary = 1 << 3,
        Mini = 1 << 4
    }


    internal static class PolisherTypeBitwiseExts {
        public static List<PolisherType> ToList(this PolisherTypeBitwise bitwise) {
            List<PolisherType> l = new();

            if (bitwise.HasFlag(PolisherTypeBitwise.DualAction)) {
                l.Add(PolisherType.DualAction);
            }

            if (bitwise.HasFlag(PolisherTypeBitwise.LongThrow)) {
                l.Add(PolisherType.LongThrow);
            }

            if (bitwise.HasFlag(PolisherTypeBitwise.ForcedRotation)) {
                l.Add(PolisherType.ForcedRotation);
            }

            if (bitwise.HasFlag(PolisherTypeBitwise.Rotary)) {
                l.Add(PolisherType.Rotary);
            }

            if (bitwise.HasFlag(PolisherTypeBitwise.Mini)) {
                l.Add(PolisherType.Mini);
            }

            return l;
        }
    }
}