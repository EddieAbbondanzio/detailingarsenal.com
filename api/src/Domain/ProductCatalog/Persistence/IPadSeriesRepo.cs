using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IPadSeriesRepo : IRepo<PadSeries> {
        Task<PadSeries?> FindByName(string name);
    }
}