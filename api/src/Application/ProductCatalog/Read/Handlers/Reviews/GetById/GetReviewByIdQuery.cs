using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetReviewByIdQuery : IAction {
        public Guid Id { get; }

        public GetReviewByIdQuery(Guid id) {
            Id = id;
        }
    }
}