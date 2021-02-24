namespace DetailingArsenal.Application.ProductCatalog {
    public record RatingStarStat : IDataTransferObject {
        public int Star { get; }
        public int Count { get; }
        public float Percentage { get; }
        public RatingStarStat(int star, int count, float percentage) {
            Star = star;
            Count = count;
            Percentage = percentage;
        }
    }
}