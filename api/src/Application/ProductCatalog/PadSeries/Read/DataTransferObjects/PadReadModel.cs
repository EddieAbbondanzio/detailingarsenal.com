using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public class PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Category { get; }
        public string Name { get; }
        public DataUrlImage? Image { get; }

        public PadReadModel(Guid id, PadCategory category, string name, DataUrlImage? image = null) {
            Id = id;
            Category = category.Serialize();
            Name = name;
            Image = image;
        }
    }
}