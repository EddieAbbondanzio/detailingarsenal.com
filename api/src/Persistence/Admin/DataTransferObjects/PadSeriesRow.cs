using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Persistence.Shared;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog {
    internal class PadSeriesRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public PolisherTypeBitwise PolisherTypes { get; set; }
    }
}