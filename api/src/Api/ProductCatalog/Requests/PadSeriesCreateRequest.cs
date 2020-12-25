using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadSeriesCreateRequest : IDataTransferObject {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
        public string Texture { get; set; } = null!;
        public string Material { get; set; } = null!;
        public List<string> PolisherTypes { get; set; } = new();
        public List<PadSizeRaw> Sizes { get; set; } = new();
        public List<PadColorCreate> Colors { get; set; } = new();
    }
}