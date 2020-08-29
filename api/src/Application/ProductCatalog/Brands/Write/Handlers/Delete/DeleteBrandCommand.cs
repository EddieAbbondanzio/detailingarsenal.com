using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class DeleteBrandCommand : IAction {
        public Guid Id { get; set; }
    }
}