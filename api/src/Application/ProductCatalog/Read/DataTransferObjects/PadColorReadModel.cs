using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadColorReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; }

        public Guid? ImageId { get; }
        public List<PadOptionReadModel> Options { get; }
        public decimal? Cut { get; }
        public decimal? Finish { get; }
        public RatingReadModel Rating { get; }

        public PadColorReadModel(Guid id, string name, string category, Guid? image, List<PadOptionReadModel> options, decimal? cut, decimal? finish, RatingReadModel rating) {
            Id = id;
            Name = name;
            Category = category;
            ImageId = image;
            Options = options;
            Cut = cut;
            Finish = finish;
            Rating = rating;
        }
    }
}