using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.ProductCatalog {
    public class BrandReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }

        public BrandReadModel(Guid id, string name) {
            Id = id;
            Name = name;
        }
    }
}