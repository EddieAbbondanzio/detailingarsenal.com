using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public List<PadCategory> Category { get; }
        public string? Material { get; }
        public string? Texture { get; }
        public string? Color { get; }
        public bool? HasCenterHole { get; }
        public Guid? ImageId { get; }
        public List<PadOptionReadModel> Options { get; }

        public PadReadModel(Guid id, string name, List<PadCategory> category, string? material, string? texture, string? color, bool? hasCenterHole, Guid? image, List<PadOptionReadModel> options) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            ImageId = image;
            Options = options;
        }
    }
}