using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "delete", Scope = "services")]
    public class DeleteServiceHandler : ActionHandler<DeleteServiceCommand> {
        private ServiceNotInUseSpecification specification;
        private IServiceRepo repo;

        public DeleteServiceHandler(ServiceNotInUseSpecification specification, IServiceRepo repo) {
            this.specification = specification;
            this.repo = repo;
        }

        public async override Task Execute(DeleteServiceCommand input, User? user) {
            var s = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            if (s.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await specification.CheckAndThrow(s);

            await repo.Delete(s);
        }
    }
}