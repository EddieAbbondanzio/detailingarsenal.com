using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesCreateCommand : IAction {
        public string Name { get; }
        public Guid BrandId { get; }
        public PadTexture Texture { get; }
        public PadMaterial Material { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<PadSize> Sizes { get; }
        public List<PadColor> Colors { get; }

        public PadSeriesCreateCommand(string name, Guid brandId, PadTexture texture, PadMaterial material, List<PolisherType> polisherTypes, List<PadSize> sizes, List<PadColor> colors) {
            Name = name;
            BrandId = brandId;
            Texture = texture;
            Material = material;
            PolisherTypes = polisherTypes;
            Sizes = sizes;
            Colors = colors;
        }
    }
}