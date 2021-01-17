using System.Text.Json.Serialization;
using DetailingArsenal;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record BrandCreateCommand : IAction {
        public string Name { get; }

        [JsonConstructor]
        public BrandCreateCommand(string name) {
            Name = name;
        }
    }
}