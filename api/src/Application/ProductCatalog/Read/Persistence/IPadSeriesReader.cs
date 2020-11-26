using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadSeriesReader {
        Task<PadSeriesReadModel?> ReadById(Guid id);
        Task<List<PadSeriesReadModel>> ReadAll();
    }
}