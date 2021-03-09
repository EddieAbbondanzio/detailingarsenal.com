using System.Collections.Generic;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadReviews : IDataTransferObject {
        public RatingReadModel Rating { get; }
        public IEnumerable<ReviewReadModel> Values { get; }
        public IEnumerable<ReviewStarStat> Stats { get; }

        public PadReviews(RatingReadModel rating, IEnumerable<ReviewReadModel> values, IEnumerable<ReviewStarStat> stats) {
            Rating = rating;
            Values = values;
            Stats = stats;
        }
    }
}