using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesUpdateCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadSeriesUpdateCommand(Guid id, string name, Guid brandId, List<PadCreateOrUpdate> pads) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}