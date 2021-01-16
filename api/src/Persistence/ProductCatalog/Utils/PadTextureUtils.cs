using System;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {

    internal static class PadTextureUtils {
        public static PadTexture Parse(string texture) => texture switch {
            "flat" => PadTexture.Flat,
            "grooved" => PadTexture.Grooved,
            "dimpled" => PadTexture.Dimpled,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PadTexture texture) => texture switch {
            PadTexture.Flat => "flat",
            PadTexture.Grooved => "grooved",
            PadTexture.Dimpled => "dimpled",
            _ => throw new NotSupportedException()
        };
    }
}