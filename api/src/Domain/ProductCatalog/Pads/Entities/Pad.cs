using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Pad : Entity<Pad> {
        public string Name { get; set; }
        public PadCategory Category { get; set; }
        public PadMaterial Material { get; set; }
        public PadTexture Texture { get; set; }
        public List<PadSize> Sizes { get; set; }
        public List<PolisherType> PolisherTypes { get; set; }
        public DataUrlImage? Image { get; set; }

        public Pad(string name, PadCategory category, PadMaterial material, PadTexture texture, List<PadSize> sizes, List<PolisherType> polisherTypes, DataUrlImage? image = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Sizes = sizes;
            PolisherTypes = polisherTypes;
            Image = image;
        }

        public Pad(Guid id, string name, PadCategory category, PadMaterial material, PadTexture texture, List<PadSize> sizes, List<PolisherType> polisherTypes, DataUrlImage? image = null) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Sizes = sizes;
            PolisherTypes = polisherTypes;
            Image = image;
        }
    }
}