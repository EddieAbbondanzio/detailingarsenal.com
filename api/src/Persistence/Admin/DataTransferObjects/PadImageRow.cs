using System;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog {
    internal class PadImageRow : IDataTransferObject {
        public Guid PadId { get; set; }
        public Guid ImageId { get; set; }
    }
}