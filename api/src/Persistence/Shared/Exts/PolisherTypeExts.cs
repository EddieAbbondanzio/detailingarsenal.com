using System.Collections.Generic;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {
    internal static class PolisherTypeExts {
        public static PolisherTypeBitwise Flatten(this List<PolisherType> polisherTypes) {
            var bitwise = PolisherTypeBitwise.None;

            for (int i = 0; i < polisherTypes.Count; i++) {
                switch (polisherTypes[i]) {
                    case PolisherType.DualAction:
                        bitwise |= PolisherTypeBitwise.DualAction;
                        break;
                    case PolisherType.LongThrow:
                        bitwise |= PolisherTypeBitwise.LongThrow;
                        break;
                    case PolisherType.ForcedRotation:
                        bitwise |= PolisherTypeBitwise.ForcedRotation;
                        break;
                    case PolisherType.Mini:
                        bitwise |= PolisherTypeBitwise.Mini;
                        break;
                    case PolisherType.Rotary:
                        bitwise |= PolisherTypeBitwise.Rotary;
                        break;
                }
            }

            return bitwise;
        }
    }
}