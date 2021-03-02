using System.Threading.Tasks;

namespace DetailingArsenal.Application.ProductCatalog {
    public interface IPadFilterReader {
        Task<PadFilterReadModel> Read();
    }
}