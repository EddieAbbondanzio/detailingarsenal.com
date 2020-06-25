using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Security {
    public interface IRoleRepo : IRepo<Role> {
        Task<List<Role>> FindAll();
        Task<Role?> Find(string name);
        Task<List<Role>> FindForUser(User user);
        Task AddToUser(User user, Role role);
        Task RemoveFromUser(User user, Role role);
    }
}