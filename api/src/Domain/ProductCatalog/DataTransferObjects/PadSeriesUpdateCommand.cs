using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record PadSeriesUpdateCommand : IAction {
        public Guid Id { get; }
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<PadSizeCreateOrUpdate> Sizes { get; }
        public List<PadColorCreateOrUpdate> Colors { get; }

        [JsonConstructor]
        public PadSeriesUpdateCommand(Guid id, string name, Guid brandId, List<PolisherType> polisherTypes, List<PadSizeCreateOrUpdate> sizes, List<PadColorCreateOrUpdate> colors) {
            Id = id;
            Name = name;
            BrandId = brandId;
            PolisherTypes = polisherTypes;
            Sizes = sizes;
            Colors = colors;
        }
    }
}