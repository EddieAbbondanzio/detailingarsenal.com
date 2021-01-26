using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadColor : Entity<PadColor> {
        public string Name { get; set; }
        public PadCategory Category { get; set; }
        public PadMaterial Material { get; set; }
        public PadTexture Texture { get; set; }
        public ProcessedImage? Image { get; set; }
        public List<PadOption> Options { get; set; }

        public PadColor(string name, PadCategory category, PadMaterial material, PadTexture texture, ProcessedImage? image = null, List<PadOption>? options = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Image = image;
            Options = options ?? new();
        }

        public PadColor(Guid id, string name, PadCategory category, PadMaterial material, PadTexture texture, ProcessedImage? image = null, List<PadOption>? options = null) {
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