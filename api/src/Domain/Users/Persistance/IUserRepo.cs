using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users {
    public interface IUserRepo : IRepo<User> {
        Task<User?> FindByAuth0Id(string id);
        Task<User?> FindByEmail(string email);
    }
}