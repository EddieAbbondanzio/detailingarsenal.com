using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadSizeReader {
        Task<List<PadSizeReadModel>> ReadSizesForPad(Guid padId);
    }
}