using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadSummaryReader {
        Task<PadSummaryReadModel?> Read(Guid id);
        Task<PagedCollection<PadSummaryReadModel>> ReadAll();
    }
}