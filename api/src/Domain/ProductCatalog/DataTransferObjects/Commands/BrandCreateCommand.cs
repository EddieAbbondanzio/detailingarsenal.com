using DetailingArsenal;

namespace DetailingArsenal.Domain.ProductCatalog {
    public record BrandCreateCommand(string Name) : IAction;
}