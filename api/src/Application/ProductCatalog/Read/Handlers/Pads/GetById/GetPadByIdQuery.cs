using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetPadByIdQuery : IAction {
        public Guid Id { get; }

        public GetPadByIdQuery(Guid id) {
            Id = id;
        }
    }
}