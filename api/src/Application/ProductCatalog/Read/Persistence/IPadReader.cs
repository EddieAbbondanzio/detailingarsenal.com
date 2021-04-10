using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadReader {
        Task<PadReadModel?> Read(Guid id);
        Task<PagedCollection<PadReadModel>> ReadAll();
        Task<PagedCollection<PadReadModel>> ReadFiltered(GetPadsFilteredQuery query);
    }
}