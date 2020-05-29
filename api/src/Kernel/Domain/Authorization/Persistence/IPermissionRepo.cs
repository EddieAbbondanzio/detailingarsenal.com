using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPermissionRepo : IRepo<Permission> {
    Task<List<Permission>> FindAll();

    Task<Permission?> Find(string action, string scope);
}