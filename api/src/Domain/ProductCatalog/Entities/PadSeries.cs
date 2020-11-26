using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeries : Aggregate<PadSeries> {
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public List<PadSeriesSize> Sizes { get; set; }
        public List<Pad> Pads { get; set; }

        public PadSeries(Guid id, string name, Guid brandId, List<PadSeriesSize>? sizes = null, List<Pad>? pads = null) {
            Id = id;
            Name = name;
            BrandId = brandId;
            Sizes = sizes ?? new List<PadSeriesSize>();
            Pads = pads ?? new List<Pad>();
        }

        public PadSeries(string name, Guid brandId, List<PadSeriesSize>? sizes = null, List<Pad>? pads = null) {
            Id = Guid.NewGuid();
            Name = name;
            BrandId = brandId;
            Sizes = sizes ?? new List<PadSeriesSize>();
            Pads = pads ?? new List<Pad>();
        }
    }
}