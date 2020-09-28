namespace DetailingArsenal.Application.ProductCatalog {
    public class RatingReadModel : IDataTransferObject {
        public int Stars { get; }
        public int ReviewCount { get; }

        public RatingReadModel(int stars, int reviewCount) {
            Stars = stars;
            ReviewCount = reviewCount;
        }
    }
}