using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IPadSeriesReader {
        Task<List<PadSeriesReadModel>> ReadAll();
    }
}