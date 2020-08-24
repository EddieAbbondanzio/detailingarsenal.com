using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public BrandReadModel Brand { get; }
        public List<PadReadModel> Pads { get; }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, List<PadReadModel> pads) {
            Id = id;
            Name = name;
            Brand = brand;
            Pads = pads;
        }
    }
}