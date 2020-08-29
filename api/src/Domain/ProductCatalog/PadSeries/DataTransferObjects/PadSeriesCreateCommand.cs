using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesCreateCommand : IAction {
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadSeriesCreateCommand(string name, Guid brandId, List<PadCreateOrUpdate> pads) {
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}