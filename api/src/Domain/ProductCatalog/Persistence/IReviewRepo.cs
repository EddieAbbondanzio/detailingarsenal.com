using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IReviewRepo : IRepo<Review> {
        Task<bool> HasReviewByUserForPad(Guid userId, Guid padId);
    }
}