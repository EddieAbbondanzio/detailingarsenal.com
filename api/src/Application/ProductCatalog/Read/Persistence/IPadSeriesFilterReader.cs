using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadSeriesFilterReader {
        Task<PadSeriesFilterReadModel> Read();
    }
}