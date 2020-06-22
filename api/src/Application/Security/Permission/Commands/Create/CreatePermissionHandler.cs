using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Validation(typeof(CreatePermissionValidator))]
    [Authorization(Action = "create", Scope = "permissions")]
    public class CreatePermissionHandler : ActionHandler<CreatePermissionCommand, PermissionDto> {
        private PermissionUniqueSpecification specification;
        private IPermissionRepo repo;
        private IMapper mapper;

        public CreatePermissionHandler(PermissionUniqueSpecification specification, IPermissionRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<PermissionDto> Execute(CreatePermissionCommand input, User? user) {
            var p = Permission.Create(
                input.Action,
                input.Scope
            );

            await specification.CheckAndThrow(p);

            await repo.Add(p);
            return mapper.Map<Permission, PermissionDto>(p);
        }
    }
}