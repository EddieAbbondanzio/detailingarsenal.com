using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public PadCategory Category { get; }
        public string Name { get; }
        public string? Image { get; }

        public PadReadModel(Guid id, PadCategory category, string name, string? image = null) {
            Id = id;
            Category = category;
            Name = name;
            Image = image;
        }
    }
}