using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    /// <summary>
    /// pad_polisher_types model
    /// </summary>
    internal class PadSeriesPolisherTypeRow : IDataTransferObject {
        public Guid PadSeriesId { get; set; }
        public string PolisherType { get; set; } = null!;
    }
}