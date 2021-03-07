using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    [DependencyInjection]
    public class NoReviewAlreadyByUser : Specification<(Guid UserId, Guid PadId)> {
        IReviewRepo repo;

        public NoReviewAlreadyByUser(IReviewRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied((Guid UserId, Guid PadId) input) {
            if (await repo.HasReviewByUserForPad(input.UserId, input.PadId)) {
                return new SpecificationResult(false, "User already has a review for the requested pad.");
            } else {
                return new SpecificationResult(true);
            }
        }
    }
}