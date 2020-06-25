using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IBusinessRepo : IRepo<Business> {
        Task<Business> FindByUser(User user);
    }
}