using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class BrandUpdateCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public BrandUpdateCommand(Guid id, string name) {
            Id = id;
            Name = name;
        }
    }
}