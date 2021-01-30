using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PadOptionPartNumberRow : IDataTransferObject {
        public Guid PadOptionId { get; set; }
        public Guid PartNumberId { get; set; }
    }
}