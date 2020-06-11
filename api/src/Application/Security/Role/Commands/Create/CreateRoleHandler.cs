using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validation(typeof(CreateRoleValidator))]
    [Authorization(Action = "create", Scope = "roles")]
    public class CreateRoleHandler : ActionHandler<CreateRoleCommand, RoleDto> {
        private RoleNameUniqueSpecification specification;
        private IRoleRepo repo;
        private IMapper mapper;

        public CreateRoleHandler(RoleNameUniqueSpecification specification, IRoleRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<RoleDto> Execute(CreateRoleCommand input, User? user) {
            var r = new Role() {
                Id = Guid.NewGuid(),
                Name = input.Name,
                PermissionIds = input.PermissionIds
            };

            await specification.CheckAndThrow(r);

            await repo.Add(r);
            return mapper.Map<Role, RoleDto>(r);
        }
    }
}