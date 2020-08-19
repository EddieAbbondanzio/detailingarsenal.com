using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IBrandReader : IReader {
        Task<List<BrandReadModel>> ReadAll();
    }
}