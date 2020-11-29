namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesSizeReadModel(float Diameter, float? Thickness, string? PartNumber) : IAction;
}