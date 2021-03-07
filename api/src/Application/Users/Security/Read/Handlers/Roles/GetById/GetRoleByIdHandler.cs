using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "read", Scope = "roles")]
    [DependencyInjection]
    public class GetRoleByIdHandler : ActionHandler<GetRoleByIdQuery, RoleReadModel?> {
        IRoleReader reader;

        public GetRoleByIdHandler(IRoleReader reader) {
            this.reader = reader;
        }

        public async override Task<RoleReadModel?> Execute(GetRoleByIdQuery input, User? user) {
            return await reader.ReadById(input.Id);
        }
    }
}