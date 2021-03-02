using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadSummaryReader {
        Task<PagedArray<PadSummaryReadModel>> ReadAll();
    }
}