using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Pad : Entity<Pad> {
        public string Name { get; set; }
        public PadCategory Category { get; set; }
        public PadMaterial Material { get; set; }
        public PadTexture Texture { get; set; }
        public List<PolisherType> PolisherTypes { get; set; }
        public DataUrlImage? Image { get; set; }

        public Pad(string name, PadCategory category, PadMaterial material, PadTexture texture, List<PolisherType>? polisherTypes = null, DataUrlImage? image = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            PolisherTypes = polisherTypes ?? new List<PolisherType>();
            Image = image;
        }

        public Pad(Guid id, string name, PadCategory category, PadMaterial material, PadTexture texture, List<PolisherType>? polisherTypes = null, DataUrlImage? image = null) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            PolisherTypes = new List<PolisherType>();
            Image = image;
        }
    }
}