using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public interface IPadSeriesReader {
        Task<PadSeriesReadModel?> ReadById(Guid id);
        Task<PagedCollection<PadSeriesReadModel>> ReadAll(PagingOptions paging);
    }
}