using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Validation(typeof(ReviewCreateHandler))]
    [Authorization(Scope = "reviews", Action = "create")]
    public class ReviewCreateHandler : ActionHandler<ReviewCreateCommand, CommandResult> {
        IReviewRepo repo;

        public ReviewCreateHandler(IReviewRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(ReviewCreateCommand input, User? user) {
            var review = new Review(user!.Id, input.PadId, new DateTime(), input.Stars, input.Cut, input.Finish, input.Title, input.Body);

            await repo.Add(review);
            return CommandResult.Insert(review.Id);
        }
    }
}