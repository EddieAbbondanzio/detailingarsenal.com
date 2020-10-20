using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadSeriesCreateRequest : IDataTransferObject {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
        public List<PadSeriesSizeRaw> Sizes { get; set; } = new List<PadSeriesSizeRaw>();
        public List<PadCreateOrUpdateRaw> Pads { get; set; } = new List<PadCreateOrUpdateRaw>();
    }
}