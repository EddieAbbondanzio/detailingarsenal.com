using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeries : Aggregate<PadSeries> {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
        public List<Pad> Pads { get; set; } = new List<Pad>();

        public PadSeries() { }

        public PadSeries(Guid id, string name, Guid brandId, List<Pad>? pads = null) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Pads = pads ?? new List<Pad>();
        }

        public PadSeries(string name, Guid brandId, List<Pad>? pads = null) {
            Id = Guid.NewGuid();
            Name = name;
            BrandId = brandId;
            Pads = pads ?? new List<Pad>();
        }
    }
}