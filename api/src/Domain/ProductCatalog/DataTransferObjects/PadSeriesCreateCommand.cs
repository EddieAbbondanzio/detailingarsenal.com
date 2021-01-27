using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesCreateCommand : IAction {
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<PadSizeCreateOrUpdate> Sizes { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        [JsonConstructor]
        public PadSeriesCreateCommand(string name, Guid brandId, List<PolisherType> polisherTypes, List<PadSizeCreateOrUpdate> sizes, List<PadCreateOrUpdate> pads) {
            Name = name;
            BrandId = brandId;
            PolisherTypes = polisherTypes;
            Sizes = sizes;
            Pads = pads;
        }
    }
}