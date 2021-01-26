using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public BrandReadModel Brand { get; }
        public List<string> PolisherTypes { get; }
        public List<PadSizeReadModel> Sizes { get; }
        public List<PadColorReadModel> Colors { get; }

        public PadSeriesReadModel(Guid id, string name, BrandReadModel brand, List<string>? polisherTypes = null, List<PadSizeReadModel>? sizes = null, List<PadColorReadModel>? colors = null) {
            Id = id;
            Name = name;
            Brand = brand;
            PolisherTypes = polisherTypes ?? new();
            Sizes = sizes ?? new();
            Colors = colors ?? new();
        }
    }
}