using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users.Security {
    public interface IPermissionRepo : IRepo<Permission> {
        Task<List<Permission>> FindAll();
        Task<Permission?> Find(string action, string scope);
        Task<PermissionSet> FindForRoles(IEnumerable<Role> roles);
        Task<bool> IsInUse(Permission permission);
    }
}