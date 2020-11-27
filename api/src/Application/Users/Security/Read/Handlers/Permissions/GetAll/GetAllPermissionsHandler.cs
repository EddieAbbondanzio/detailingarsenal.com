
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "read", Scope = "permissions")]
    public class GetAllPermissionsHandler : ActionHandler<GetAllPermissionsQuery, List<PermissionReadModel>> {
        IPermissionReader reader;

        public GetAllPermissionsHandler(IPermissionReader reader) {
            this.reader = reader;
        }

        public async override Task<List<PermissionReadModel>> Execute(GetAllPermissionsQuery input, User? user) {
            var perms = await reader.ReadAll();
            return perms;
        }
    }
}