using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum PolisherType {
        DualAction,
        LongThrow,
        ForcedRotation,
        Rotary
    }

    public static class PolisherTypeUtils {
        public static PolisherType Parse(string type) => type switch
        {
            "dual_action" => PolisherType.DualAction,
            "long_throw" => PolisherType.LongThrow,
            "forced_rotation" => PolisherType.ForcedRotation,
            "rotary" => PolisherType.Rotary,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PolisherType type) => type switch
        {
            PolisherType.DualAction => "dual_action",
            PolisherType.LongThrow => "long_throw",
            PolisherType.ForcedRotation => "forced_rotation",
            PolisherType.Rotary => "rotary",
            _ => throw new NotSupportedException()
        };
    }
}