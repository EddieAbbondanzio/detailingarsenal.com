using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadSeriesUpdateRequest : IDataTransferObject {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
        public List<PadData> Pads { get; set; } = new List<PadData>();
    }
}