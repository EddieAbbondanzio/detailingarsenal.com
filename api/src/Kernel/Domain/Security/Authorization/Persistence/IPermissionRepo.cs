using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPermissionRepo : IRepo<Permission> {
    Task<List<Permission>> FindAll();
    Task<Permission?> Find(string action, string scope);
    Task<List<Permission>> FindForRoles(IEnumerable<Role> roles);
    Task<bool> IsInUse(Permission permission);
}