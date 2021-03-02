using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetPadSeriesByIdQuery : IAction {
        public Guid Id { get; }

        public GetPadSeriesByIdQuery(Guid id) {
            Id = id;
        }
    }
}