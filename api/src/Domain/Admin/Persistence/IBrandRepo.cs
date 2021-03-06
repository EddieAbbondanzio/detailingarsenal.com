using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public interface IBrandRepo : IRepo<Brand> {
        Task<Brand?> FindByName(string name);
        Task<bool> IsBrandInUse(Brand b);
    }
}