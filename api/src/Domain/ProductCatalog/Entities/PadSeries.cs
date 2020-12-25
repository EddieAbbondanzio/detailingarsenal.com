using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeries : Aggregate<PadSeries> {
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public PadMaterial Material { get; set; }
        public PadTexture Texture { get; set; }

        /// <summary>
        /// Recommended for the following polisher types.
        /// </summary>
        public List<PolisherType> PolisherTypes { get; set; }
        public List<PadSize> Sizes { get; set; }
        public List<PadColor> Colors { get; set; }

        public PadSeries(Guid id, string name, Guid brandId, PadMaterial material, PadTexture texture, List<PolisherType> polisherTypes, List<PadSize>? sizes = null, List<PadColor>? colors = null) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Material = material;
            Texture = texture;
            PolisherTypes = polisherTypes;
            Sizes = sizes ?? new();
            Colors = colors ?? new();
        }

        public PadSeries(string name, Guid brandId, PadMaterial material, PadTexture texture, List<PolisherType> polisherTypes, List<PadSize>? sizes = null, List<PadColor>? colors = null) {
            Id = Guid.NewGuid();
            Name = name;
            BrandId = brandId;
            Material = material;
            Texture = texture;
            PolisherTypes = polisherTypes;
            Sizes = sizes ?? new();
            Colors = colors ?? new();
        }
    }
}