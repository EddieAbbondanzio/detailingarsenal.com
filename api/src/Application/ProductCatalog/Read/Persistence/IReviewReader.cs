using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IReviewReader {
        Task<ReviewReadModel?> ReadById(Guid id);
        Task<PadReviews> ReadForPad(Guid padId);
    }
}