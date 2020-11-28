using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.Users.Security {
    public interface IRoleReader {
        Task<List<RoleReadModel>> ReadAll();
        Task<RoleReadModel?> ReadById(Guid id);
        Task<RoleReadModel?> ReadByName(string name);
    }
}