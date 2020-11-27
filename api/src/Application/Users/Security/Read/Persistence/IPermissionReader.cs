using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.Users.Security {
    public interface IPermissionReader {
        Task<List<PermissionReadModel>> ReadAll();
    }
}