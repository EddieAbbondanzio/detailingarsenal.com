using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesUpdateCommand : IAction {
        public Guid Id { get; }
        public string Name { get; }
        public Guid BrandId { get; }
        public PolisherType PolisherTypes { get; }
        public List<PadSizeCreateOrUpdate> Sizes { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        [JsonConstructor]
        public PadSeriesUpdateCommand(Guid id, string name, Guid brandId, PolisherType polisherTypes, List<PadSizeCreateOrUpdate> sizes, List<PadCreateOrUpdate> pads) {
            Id = id;
            Name = name;
            BrandId = brandId;
            PolisherTypes = polisherTypes;
            Sizes = sizes;
            Pads = pads;
        }
    }
}