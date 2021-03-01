using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Pad : Entity<Pad> {
        public string Name { get; set; }
        public List<PadCategory> Category { get; set; }
        public PadMaterial? Material { get; set; }
        public PadTexture? Texture { get; set; }
        public PadColor? Color { get; set; }
        public bool? HasCenterHole { get; set; }
        public ProcessedImage? Image { get; set; }
        public List<PadOption> Options { get; set; }

        public Pad(string name, PadCategory category, PadMaterial? material, PadTexture? texture, PadColor? color, bool? hasCenterHole, ProcessedImage? image = null, List<PadOption>? options = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = new(new[] { category });
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            Image = image;
            Options = options ?? new();
        }

        public Pad(string name, List<PadCategory> category, PadMaterial? material, PadTexture? texture, PadColor? color, bool? hasCenterHole, ProcessedImage? image = null, List<PadOption>? options = null) {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            Image = image;
            Options = options ?? new();
        }

        public Pad(Guid id, string name, List<PadCategory> category, PadMaterial? material, PadTexture? texture, PadColor? color, bool? hasCenterHole, ProcessedImage? image = null, List<PadOption>? options = null) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            Image = image;
            Options = options ?? new();
        }
    }
}