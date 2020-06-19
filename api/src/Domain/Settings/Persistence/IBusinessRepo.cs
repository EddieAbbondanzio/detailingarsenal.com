using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    public interface IBusinessRepo : IRepo<Business> {
        Task<Business> FindByUser(User user);
    }
}