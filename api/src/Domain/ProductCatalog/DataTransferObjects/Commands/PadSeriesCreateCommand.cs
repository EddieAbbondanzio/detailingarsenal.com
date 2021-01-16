using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesCreateCommand : IAction {
        public string Name { get; }
        public Guid BrandId { get; }
        public PadTexture Texture { get; }
        public PadMaterial Material { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<PadSizeCreateOrUpdate> Sizes { get; }
        public List<PadColorCreateOrUpdate> Colors { get; }

        [JsonConstructor]
        public PadSeriesCreateCommand(string name, Guid brandId, PadTexture texture, PadMaterial material, List<PolisherType> polisherTypes, List<PadSizeCreateOrUpdate> sizes, List<PadColorCreateOrUpdate> colors) {
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