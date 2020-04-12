using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IBusinessRepo : IRepo<Business> {
        Task<Business> FindByUser(User user);
    }
}