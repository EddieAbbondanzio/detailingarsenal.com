using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Pad : Entity<Pad> {
        public string Name { get; set; }
        public PadCategory Category { get; set; }
        public PadMaterial Material { get; set; }
        public PadTexture Texture { get; set; }
        public ProcessedImage? Image { get; set; }
        public List<PadOption> Options { get; set; }

        public Pad(string name, PadCategory category, PadMaterial material, PadTexture texture, ProcessedImage? image = null, List<PadOption>? options = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Image = image;
            Options = options ?? new();
        }

        public Pad(Guid id, string name, PadCategory category, PadMaterial material, PadTexture texture, ProcessedImage? image = null, List<PadOption>? options = null) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Image = image;
            Options = options ?? new();
        }
    }
}