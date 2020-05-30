using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRoleRepo : IRepo<Role> {
    Task<List<Role>> FindAll();
}