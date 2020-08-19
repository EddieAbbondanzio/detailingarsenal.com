using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class BrandUpdate : IDataTransferObject {
        public string Name { get; }

        public BrandUpdate(string name) {
            Name = name;
        }
    }
}