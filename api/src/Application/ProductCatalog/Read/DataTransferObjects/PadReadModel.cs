using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public PadSeriesReadModel Series { get; }
        public PadBrandReadModel Brand { get; }
        public List<PadCategory> Category { get; }
        public PadMaterial? Material { get; }
        public PadTexture? Texture { get; }
        public bool HasCenterHole { get; }
        public List<PolisherType> PolisherTypes { get; }
        public float? Cut { get; }
        public float? Finish { get; }
        public PadRatingReadModel Rating { get; }

        public PadReadModel(Guid id, string name, PadSeriesReadModel series, PadBrandReadModel brand, List<PadCategory> category, PadMaterial? material, PadTexture? texture, bool hasCenterHole, List<PolisherType> polisherTypes, float? cut, float? finish, PadRatingReadModel rating) {
            Id = id;
            Name = name;
            Series = series;
            Brand = brand;
            Category = category;
            Material = material;
            Texture = texture;
            HasCenterHole = hasCenterHole;
            PolisherTypes = polisherTypes;
            Cut = cut;
            Finish = finish;
            Rating = rating;
        }
    }


    public record PadSeriesReadModel(Guid Id, string Name) : IDataTransferObject;
    public record PadBrandReadModel(Guid Id, string Name) : IDataTransferObject;
    public record PadRatingReadModel(float? Stars, int ReviewCount) : IDataTransferObject;
}