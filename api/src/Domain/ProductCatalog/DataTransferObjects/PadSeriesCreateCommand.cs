using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesCreateCommand : IAction {
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadSeriesSize> Sizes { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadSeriesCreateCommand(string name, Guid brandId, List<PadSeriesSize> sizes, List<PadCreateOrUpdate> pads) {
            Name = name;
            BrandId = brandId;
            Sizes = sizes;
            Pads = pads;
        }
    }
}