using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public class PadReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; }
        public int Cut { get; }
        public int Finish { get; }
        public string Material { get; }
        public string Texture { get; }
        public PadSizeReadModel[] Sizes { get; }
        public string[] PolisherTypes { get; }
        public RatingReadModel Rating { get; }
        public DataUrlImage? Image { get; }

        public PadReadModel(Guid id, string name, string category, int cut, int finish, string material, string texture, PadSizeReadModel[] sizes, string[] polisherTypes, RatingReadModel rating, DataUrlImage? image) {
            Id = id;
            Name = name;
            Category = category;
            Cut = cut;
            Finish = finish;
            Material = material;
            Texture = texture;
            Sizes = sizes;
            PolisherTypes = polisherTypes;
            Rating = rating;
            Image = image;
        }
    }
}