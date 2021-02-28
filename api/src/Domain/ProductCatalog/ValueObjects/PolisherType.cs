using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    [Flags]
    public enum PolisherType {
        None = 0,
        DualAction = 1 << 0,
        LongThrow = 1 << 1,
        ForcedRotation = 1 << 2,
        Rotary = 1 << 3,
        Mini = 1 << 4
    }
}