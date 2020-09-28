using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class Rating : ValueObject<Rating> {
        public int Stars { get; }
        public int ReviewCount { get; }

        public Rating(int stars, int reviewCount) {
            Stars = stars;
            ReviewCount = reviewCount;
        }
    }
}