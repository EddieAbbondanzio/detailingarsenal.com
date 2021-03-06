using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public interface IBrandReader : IReader {
        Task<List<BrandReadModel>> ReadAll();
        Task<BrandReadModel?> ReadById(Guid id);
    }
}