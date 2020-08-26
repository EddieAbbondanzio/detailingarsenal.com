using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesUpdate : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadCreateOrUpdate> Pads { get; }

        public PadSeriesUpdate(Guid id, string name, Guid brandId, List<PadCreateOrUpdate> pads) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}