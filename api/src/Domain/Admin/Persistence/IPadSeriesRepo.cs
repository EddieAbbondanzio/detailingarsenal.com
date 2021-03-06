using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public interface IPadSeriesRepo : IRepo<PadSeries> {
        Task<PadSeries?> FindByName(string name);
    }
}