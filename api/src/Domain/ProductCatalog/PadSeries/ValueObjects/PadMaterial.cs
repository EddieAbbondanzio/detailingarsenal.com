using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum PadMaterial {
        Foam,
        Wool,
        Microfiber
    }

    public static class PadMaterialUtils {
        public static PadMaterial Parse(string material) => material switch
        {
            "foam" => PadMaterial.Foam,
            "wool" => PadMaterial.Wool,
            "microfiber" => PadMaterial.Microfiber,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PadMaterial material) => material switch
        {
            PadMaterial.Foam => "foam",
            PadMaterial.Wool => "wool",
            PadMaterial.Microfiber => "microfiber",
            _ => throw new NotSupportedException()
        };
    }
}