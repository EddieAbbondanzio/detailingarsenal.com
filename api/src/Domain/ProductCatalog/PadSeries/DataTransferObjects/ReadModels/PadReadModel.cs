using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public PadCategory Category { get; }
        public string Name { get; }
        public Base64Image? Image { get; }

        public PadReadModel(Guid id, PadCategory category, string name, Base64Image? image = null) {
            Id = id;
            Category = category;
            Name = name;
            Image = image;
        }
    }
}