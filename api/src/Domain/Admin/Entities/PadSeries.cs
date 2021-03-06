using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public class PadSeries : Aggregate<PadSeries> {
        public string Name { get; set; }
        public Guid BrandId { get; set; }

        /// <summary>
        /// Recommended for the following polisher types.
        /// </summary>
        public List<PolisherType> PolisherTypes { get; set; }
        public List<PadSize> Sizes { get; set; }
        public List<Pad> Pads { get; set; }

        public PadSeries(Guid id, string name, Guid brandId, List<PolisherType>? polisherTypes = null, List<PadSize>? sizes = null, List<Pad>? colors = null) {
            Id = id;
            Name = name;
            BrandId = brandId;
            PolisherTypes = polisherTypes ?? new();
            Sizes = sizes ?? new();
            Pads = colors ?? new();
        }

        public PadSeries(string name, Guid brandId, List<PolisherType>? polisherTypes = null, List<PadSize>? sizes = null, List<Pad>? colors = null) {
            Id = Guid.NewGuid();
            Name = name;
            BrandId = brandId;
            PolisherTypes = polisherTypes ?? new();
            Sizes = sizes ?? new();
            Pads = colors ?? new();
        }
    }
}