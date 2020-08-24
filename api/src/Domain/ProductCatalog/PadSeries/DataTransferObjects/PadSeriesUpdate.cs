using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesUpdate : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadUpdate> Pads { get; }

        public PadSeriesUpdate(Guid id, string name, Guid brandId, List<PadUpdate> pads) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}