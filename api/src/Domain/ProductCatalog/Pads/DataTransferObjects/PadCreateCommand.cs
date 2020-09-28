using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateCommand : IAction {
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadCreateCommand(string name, Guid brandId, List<PadCreateOrUpdate> pads) {
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}