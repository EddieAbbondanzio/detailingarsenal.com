using System;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal static class PadMaterialUtils {
        public static PadMaterial Parse(string material) => material switch {
            "foam" => PadMaterial.Foam,
            "wool" => PadMaterial.Wool,
            "microfiber" => PadMaterial.Microfiber,
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