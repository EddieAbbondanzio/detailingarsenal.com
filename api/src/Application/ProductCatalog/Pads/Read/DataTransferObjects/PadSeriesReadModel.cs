using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.ProductCatalog {
    public class PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; } = null!;
        public BrandReadModel Brand { get; } = null!;
        public List<PadSeriesSizeReadModel> Sizes = new List<PadSeriesSizeReadModel>();
        public List<PadReadModel> Pads { get; set; } = new List<PadReadModel>();

        public PadSeriesReadModel() {
            // Needed for AutoMapper
        }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, List<PadSeriesSizeReadModel>? series = null, List<PadReadModel>? pads = null) {
            Id = id;
            Name = name;
            Brand = brand;
            Sizes = Sizes ?? new List<PadSeriesSizeReadModel>();
            Pads = pads ?? new List<PadReadModel>();
        }
    }
}