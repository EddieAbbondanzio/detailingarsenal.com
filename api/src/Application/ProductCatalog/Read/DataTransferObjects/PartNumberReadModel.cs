namespace DetailingArsenal.Application.ProductCatalog {
    public record PartNumberReadModel(string Value, string? Notes) : IDataTransferObject;
}