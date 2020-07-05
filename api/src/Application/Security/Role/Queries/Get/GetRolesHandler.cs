
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "read", Scope = "roles")]
    public class GetRolesHandler : ActionHandler<GetRolesQuery, List<RoleView>> {
        IRoleService service;
        IMapper mapper;

        public GetRolesHandler(IRoleService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<RoleView>> Execute(GetRolesQuery input, User? user) {
            List<Role> perms = await service.GetAll();
            return mapper.Map<List<Role>, List<RoleView>>(perms);
        }
    }
}