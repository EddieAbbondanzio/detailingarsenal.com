using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IReviewReader {
        Task<ReviewReadModel?> ReadById(Guid id);
        Task<List<ReviewReadModel>> ReadForPad(Guid padId);
    }
}