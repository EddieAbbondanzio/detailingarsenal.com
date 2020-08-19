using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IBrandRepo : IRepo<Brand> {
        Task<Brand> FindByName(string name);
    }
}