using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesUpdateCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadSeriesSize> Sizes { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadSeriesUpdateCommand(Guid id, string name, Guid brandId, List<PadSeriesSize> sizes, List<PadCreateOrUpdate> pads) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Sizes = sizes;
            Pads = pads;
        }
    }
}