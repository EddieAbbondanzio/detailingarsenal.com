using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; }
        public int? Cut { get; }
        public int? Finish { get; }
        public string Material { get; }
        public string Texture { get; }
        public List<string> PolisherTypes { get; }
        public RatingReadModel Rating { get; }
        public DataUrlImage? Image { get; }

        public PadReadModel(Guid id, string name, string category, int? cut, int? finish, string material, string texture, List<string>? polisherTypes = null, RatingReadModel? rating = null, DataUrlImage? image = null) {
            Id = id;
            Name = name;
            Category = category;
            Cut = cut;
            Finish = finish;
            Material = material;
            Texture = texture;
            PolisherTypes = polisherTypes ?? new List<string>();
            Rating = rating ?? RatingReadModel.Empty();
            Image = image;
        }
    }
}