using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {

    [DependencyInjection]
    public class GetAllReviewsForPadHandler : ActionHandler<GetAllReviewsForPadQuery, PadReviews> {
        IReviewReader reader;

        public GetAllReviewsForPadHandler(IReviewReader reader) {
            this.reader = reader;
        }

        public async override Task<PadReviews> Execute(GetAllReviewsForPadQuery input, User? user) {
            var reviews = await reader.ReadForPad(input.PadId);
            return reviews;
        }
    }
}