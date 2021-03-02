using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Application.ProductCatalog {
    public record BrandUpdateCommand : IAction {
        public Guid Id { get; }
        public string Name { get; }

        [JsonConstructor]
        public BrandUpdateCommand(Guid id, string name) {
            Id = id;
            Name = name;
        }
    }
}