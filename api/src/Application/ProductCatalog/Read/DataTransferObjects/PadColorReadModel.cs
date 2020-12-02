using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadColorReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; }
        public DataUrlImage? Image { get; }
        public List<PadOptionReadModel> Options { get; }
        public int? Cut { get; }
        public int? Finish { get; }
        public RatingReadModel Rating { get; }

        public PadColorReadModel(Guid id, string name, string category, DataUrlImage? image, List<PadOptionReadModel> options, int? cut, int? finish, RatingReadModel rating) {
            Id = id;
            Name = name;
            Category = category;
            Image = image;
            Options = options;
            Cut = cut;
            Finish = finish;
            Rating = rating;
        }
    }
}