using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validation(typeof(UpdatePermissionValidator))]
    [Authorization(Action = "update", Scope = "permissions")]
    public class UpdatePermissionHandler : ActionHandler<UpdatePermissionCommand, PermissionDto> {
        private PermissionUniqueSpecification specification;
        private IPermissionRepo repo;
        private IMapper mapper;

        public UpdatePermissionHandler(PermissionUniqueSpecification specification, IPermissionRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<PermissionDto> Execute(UpdatePermissionCommand input, User? user) {
            var p = await repo.FindById(input.Id);

            if (p == null) {
                throw new EntityNotFoundException();
            }

            p.Action = input.Action;
            p.Scope = input.Scope;

            await specification.CheckAndThrow(p);

            await repo.Update(p);
            return mapper.Map<Permission, PermissionDto>(p);
        }
    }
}