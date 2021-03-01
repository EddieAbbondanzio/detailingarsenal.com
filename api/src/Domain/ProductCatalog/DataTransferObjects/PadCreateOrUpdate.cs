using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public string Name { get; }
        public List<PadCategory> Category { get; }
        public PadMaterial? Material { get; }
        public PadTexture? Texture { get; }
        public PadColor? Color { get; }
        public bool? HasCenterHole { get; }
        public Either<Guid, DataUrlImage>? Image { get; }
        public List<PadOptionCreateOrUpdate> Options { get; }

        public PadCreateOrUpdate(Guid? id, string name, PadCategory category, PadMaterial? material, PadTexture? texture, PadColor? color, bool? hasCenterHole, Either<Guid, DataUrlImage>? image, List<PadOptionCreateOrUpdate> options) {
            Id = id;
            Name = name;
            Category = new(new[] { category });
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            Image = image;
            Options = options;
        }

        [JsonConstructor]
        public PadCreateOrUpdate(Guid? id, string name, List<PadCategory> category, PadMaterial? material, PadTexture? texture, PadColor? color, bool? hasCenterHole, Either<Guid, DataUrlImage>? image, List<PadOptionCreateOrUpdate> options) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            Image = image;
            Options = options;
        }
    }
}