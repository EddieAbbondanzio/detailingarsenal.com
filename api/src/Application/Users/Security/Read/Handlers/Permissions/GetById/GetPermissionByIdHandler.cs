using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "read", Scope = "permissions")]
    [DependencyInjection]
    public class GetPermissionByIdHandler : ActionHandler<GetPermissionByIdQuery, PermissionReadModel?> {
        IPermissionReader reader;

        public GetPermissionByIdHandler(IPermissionReader reader) {
            this.reader = reader;
        }

        public async override Task<PermissionReadModel?> Execute(GetPermissionByIdQuery input, User? user) {
            var p = await reader.ReadById(input.Id);
            return p;
        }
    }
}