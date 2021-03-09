namespace DetailingArsenal.Application.ProductCatalog {
    public record ReviewStarStat(int Stars, int Count, float Percentage) : IDataTransferObject;
}