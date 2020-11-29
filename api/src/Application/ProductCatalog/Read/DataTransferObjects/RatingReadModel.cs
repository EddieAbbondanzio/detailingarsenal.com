namespace DetailingArsenal.Application.ProductCatalog {
    public record RatingReadModel : IDataTransferObject {
        public int Stars { get; }
        public int ReviewCount { get; }

        public RatingReadModel(int stars, int reviewCount) {
            Stars = stars;
            ReviewCount = reviewCount;
        }

        public static RatingReadModel Empty() => new RatingReadModel(0, 0);
    }
}