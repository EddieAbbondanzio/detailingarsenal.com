using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetAllReviewsForPadQuery : IAction {
        public Guid PadId { get; }

        public GetAllReviewsForPadQuery(Guid padId) {
            PadId = padId;
        }
    }
}