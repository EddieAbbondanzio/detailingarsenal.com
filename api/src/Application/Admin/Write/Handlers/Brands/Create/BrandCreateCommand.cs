using System.Text.Json.Serialization;
using DetailingArsenal;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public record BrandCreateCommand : IAction {
        public string Name { get; }

        [JsonConstructor]
        public BrandCreateCommand(string name) {
            Name = name;
        }
    }
}