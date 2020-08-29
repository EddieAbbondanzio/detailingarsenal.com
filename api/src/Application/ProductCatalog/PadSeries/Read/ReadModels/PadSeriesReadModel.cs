using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.ProductCatalog {
    public class PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; } = null!;
        public BrandReadModel Brand { get; } = null!;
        public List<PadReadModel> Pads { get; } = new List<PadReadModel>();

        public PadSeriesReadModel() {
            // Needed for AutoMapper
        }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, List<PadReadModel>? pads = null) {
            Id = id;
            Name = name;
            Brand = brand;
            Pads = pads ?? new List<PadReadModel>();
        }
    }
}