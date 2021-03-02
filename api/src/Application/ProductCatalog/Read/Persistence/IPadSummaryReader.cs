using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadSummaryReader {
        Task<PagedCollection<PadSummaryReadModel>> ReadAll();
    }
}