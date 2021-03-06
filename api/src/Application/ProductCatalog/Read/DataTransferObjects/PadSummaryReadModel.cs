using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSummaryReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public PadSummarySeriesReadModel Series { get; }
        public PadSummaryBrandReadModel Brand { get; }
        public List<PadCategory> Category { get; }
        public PadMaterial Material { get; }
        public PadTexture Texture { get; }
        public bool HasCenterHole { get; }
        public List<PolisherType> PolisherTypes { get; }
        public float? Cut { get; }
        public float? Finish { get; }
        public PadSummaryRatingReadModel Rating { get; }

        public PadSummaryReadModel(Guid id, string name, PadSummarySeriesReadModel series, PadSummaryBrandReadModel brand, List<PadCategory> category, PadMaterial material, PadTexture texture, bool hasCenterHole, List<PolisherType> polisherTypes, float? cut, float? finish, PadSummaryRatingReadModel rating) {
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


    public record PadSummarySeriesReadModel(Guid Id, string Name) : IDataTransferObject;
    public record PadSummaryBrandReadModel(Guid Id, string Name) : IDataTransferObject;
    public record PadSummaryRatingReadModel(float? Stars, int ReviewCount) : IDataTransferObject;
}