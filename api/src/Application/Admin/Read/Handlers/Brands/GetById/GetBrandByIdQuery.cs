using System;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public class GetBrandByIdQuery : IAction {
        public Guid Id { get; }

        public GetBrandByIdQuery(Guid id) {
            Id = id;
        }
    }
}