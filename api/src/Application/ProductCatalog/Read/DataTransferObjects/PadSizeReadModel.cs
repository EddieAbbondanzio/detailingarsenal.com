namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSizeReadModel(float Diameter, float? Thickness) : IDataTransferObject;
}