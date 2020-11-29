using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public BrandReadModel Brand { get; }
        public List<PadSeriesSizeReadModel> Sizes { get; }
        public List<PadReadModel> Pads { get; }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, List<PadSeriesSizeReadModel>? series = null, List<PadSeriesSizeReadModel>? sizes = null, List<PadReadModel>? pads = null) {
            Id = id;
            Name = name;
            Brand = brand;
            Sizes = sizes ?? new List<PadSeriesSizeReadModel>();
            Pads = pads ?? new List<PadReadModel>();
        }
    }
}