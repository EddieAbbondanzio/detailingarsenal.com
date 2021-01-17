using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesUpdateCommand : IAction {
        public Guid Id { get; }
        public string Name { get; }
        public Guid BrandId { get; }
        public PadTexture Texture { get; }
        public PadMaterial Material { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<PadSizeCreateOrUpdate> Sizes { get; }
        public List<PadColorCreateOrUpdate> Colors { get; }

        public PadSeriesUpdateCommand(Guid id, string name, Guid brandId, PadTexture texture, PadMaterial material, List<PolisherType> polisherTypes, List<PadSizeCreateOrUpdate> sizes, List<PadColorCreateOrUpdate> colors) {
            Id = id;
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