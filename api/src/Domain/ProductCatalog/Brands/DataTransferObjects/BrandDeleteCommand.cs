using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class BrandDeleteCommand : IAction {
        public Guid Id { get; set; }

        public BrandDeleteCommand(Guid id) {
            Id = id;
        }
    }
}