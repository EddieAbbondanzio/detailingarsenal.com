using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    /// <summary>
    /// pad_polisher_types model
    /// </summary>
    public class PadPolisherTypeRow : IDataTransferObject {
        public Guid PadId { get; set; }
        public string PolisherType { get; set; } = null!;
    }
}