using System.Collections.Generic;
using System.Linq;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    /// <summary>
    /// Enum free raw variant so we can deserialize our dirty snake 
    /// case enums ourselves.
    /// </summary>
    public class PadCreateOrUpdateRaw : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string Texture { get; set; } = null!;
        public List<PadSeriesSize> Sizes { get; set; } = new List<PadSeriesSize>();
        public List<string> PolisherTypes { get; set; } = new List<string>();
        public DataUrlImage? Image { get; set; }

        public PadCreateOrUpdate ToReal() => new PadCreateOrUpdate(
            Name,
            PadCategoryUtils.Parse(Category),
            PadMaterialUtils.Parse(Material),
            PadTextureUtils.Parse(Texture),
            Sizes,
            PolisherTypes.Select(pt => PolisherTypeUtils.Parse(pt)).ToList(),
            Image
        );
    }
}