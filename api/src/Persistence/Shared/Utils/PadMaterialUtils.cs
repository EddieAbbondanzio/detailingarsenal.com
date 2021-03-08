using System;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {
    internal static class PadMaterialUtils {
        public static PadMaterial? Parse(string? material) => material switch {
            "foam" => PadMaterial.Foam,
            "wool" => PadMaterial.Wool,
            "microfiber" => PadMaterial.Microfiber,
            null => null,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PadMaterial material) => material switch {
            PadMaterial.Foam => "foam",
            PadMaterial.Wool => "wool",
            PadMaterial.Microfiber => "microfiber",
            _ => throw new NotSupportedException()
        };
    }
}