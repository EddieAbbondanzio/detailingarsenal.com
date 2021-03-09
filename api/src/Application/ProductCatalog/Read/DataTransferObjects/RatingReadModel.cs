namespace DetailingArsenal.Application.ProductCatalog {
    public record RatingReadModel(float? Stars, int ReviewCount) : IDataTransferObject;
}