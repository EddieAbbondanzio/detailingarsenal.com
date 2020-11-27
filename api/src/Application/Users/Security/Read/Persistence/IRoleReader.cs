using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.Users.Security {
    public interface IRoleReader {
        Task<List<RoleReadModel>> ReadAll();
    }
}