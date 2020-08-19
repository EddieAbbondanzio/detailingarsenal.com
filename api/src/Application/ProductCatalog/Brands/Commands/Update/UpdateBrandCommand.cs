using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class UpdateBrandCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}