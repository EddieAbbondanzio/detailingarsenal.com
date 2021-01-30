using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PadImageRow : IDataTransferObject {
        public Guid PadId { get; set; }
        public Guid ImageId { get; set; }
    }
}