using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Pad : Entity<Pad> {
        public PadCategory Category { get; set; }
        public string Name { get; set; } = null!;
        public Base64Image? Image { get; set; }

        public Pad() { }

        public Pad(Guid id, PadCategory category, string name, Base64Image? image = null) {
            Id = id;
            Category = category;
            Name = name;
            Image = image;
        }

        public Pad(PadCategory category, string name, Base64Image? image = null) {
            Id = Guid.NewGuid();
            Category = category;
            Name = name;
            Image = image;
        }
    }
}