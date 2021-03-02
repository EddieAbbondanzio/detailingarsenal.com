using System.Text.Json.Serialization;
using DetailingArsenal;

namespace DetailingArsenal.Application.ProductCatalog {
    public record BrandCreateCommand : IAction {
        public string Name { get; }

        [JsonConstructor]
        public BrandCreateCommand(string name) {
            Name = name;
        }
    }
}