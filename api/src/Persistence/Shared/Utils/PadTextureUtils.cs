using System;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {

    internal static class PadTextureUtils {
        public static PadTexture? Parse(string? texture) => texture switch {
            "flat" => PadTexture.Flat,
            "grooved" => PadTexture.Grooved,
            "dimpled" => PadTexture.Dimpled,
            "pile" => PadTexture.Pile,
            null => null,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PadTexture texture) => texture switch {
            PadTexture.Flat => "flat",
            PadTexture.Grooved => "grooved",
            PadTexture.Dimpled => "dimpled",
            PadTexture.Pile => "pile",
            _ => throw new NotSupportedException()
        };
    }
}