using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesCreate : IDataTransferObject {
        public string Name { get; }
        public Guid BrandId { get; }
        public List<PadCreate> Pads { get; }

        public PadSeriesCreate(string name, Guid brandId, List<PadCreate> pads) {
            Name = name;
            BrandId = brandId;
            Pads = pads;
        }
    }
}