
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "read", Scope = "roles")]
    public class GetRolesHandler : ActionHandler<GetAllRolesQuery, List<RoleReadModel>> {
        IRoleReader reader;

        public GetRolesHandler(IRoleReader reader) {
            this.reader = reader;
        }

        public async override Task<List<RoleReadModel>> Execute(GetAllRolesQuery input, User? user) {
            var roles = await reader.ReadAll();
            return roles;
        }
    }
}