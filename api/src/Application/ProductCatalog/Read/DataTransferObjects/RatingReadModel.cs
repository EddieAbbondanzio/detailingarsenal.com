using System.Collections.Generic;

namespace DetailingArsenal.Application.ProductCatalog {
    public record RatingReadModel : IDataTransferObject {
        public decimal Stars { get; }
        public int ReviewCount { get; }
        public List<RatingStarStat> Stats { get; }

        public RatingReadModel(decimal stars, int reviewCount, List<RatingStarStat>? stats = null) {
            Stars = stars;
            ReviewCount = reviewCount;
            Stats = stats ?? new();
        }

        public static RatingReadModel Empty() => new RatingReadModel(0, 0);
    }
}