using System;
using System.Collections.Generic;
using System.Linq;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public string Name { get; }
        public PadCategory Category { get; }
        public PadMaterial Material { get; }
        public PadTexture Texture { get; }
        public Either<Guid, DataUrlImage>? Image { get; }
        public List<PadOptionCreateOrUpdate> Options { get; }

        public PadCreateOrUpdate(Guid? id, string name, PadCategory category, PadMaterial material, PadTexture texture, Either<Guid, DataUrlImage>? image, List<PadOptionCreateOrUpdate> options) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Image = image;
            Options = options;
        }
    }
}