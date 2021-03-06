using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public BrandReadModel Brand { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<PadSizeReadModel> Sizes { get; }
        public List<PadReadModel> Pads { get; }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, List<PolisherType>? polisherTypes = null, List<PadSizeReadModel>? sizes = null, List<PadReadModel>? pads = null) {
            Id = id;
            Name = name;
            Brand = brand;
            PolisherTypes = polisherTypes ?? new();
            Sizes = sizes ?? new();
            Pads = pads ?? new();
        }
    }
}