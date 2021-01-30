using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; }
        public string? Material { get; }
        public string? Texture { get; }
        public string? Color { get; }
        public bool? HasCenterHole { get; }
        public Guid? ImageId { get; }
        public List<PadOptionReadModel> Options { get; }
        public decimal? Cut { get; }
        public decimal? Finish { get; }
        public RatingReadModel Rating { get; }

        public PadReadModel(Guid id, string name, string category, string? material, string? texture, string? color, bool? hasCenterHole, Guid? image, List<PadOptionReadModel> options, decimal? cut, decimal? finish, RatingReadModel rating) {
            Id = id;
            Name = name;
            Category = category;
            Material = material;
            Texture = texture;
            Color = color;
            HasCenterHole = hasCenterHole;
            ImageId = image;
            Options = options;
            Cut = cut;
            Finish = finish;
            Rating = rating;
        }
    }
}