using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    public class GetReviewByIdHandler : ActionHandler<GetReviewByIdQuery, ReviewReadModel?> {
        IReviewReader reader;

        public GetReviewByIdHandler(IReviewReader reader) {
            this.reader = reader;
        }

        public async override Task<ReviewReadModel> Execute(GetReviewByIdQuery input, User? user) {
            var r = await reader.ReadById(input.Id);
            return r;
        }
    }
}