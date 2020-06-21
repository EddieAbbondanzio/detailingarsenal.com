using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "delete", Scope = "appointments")]
    public class DeleteAppointmentHandler : ActionHandler<DeleteAppointmentCommand> {
        private IAppointmentRepo repo;

        public DeleteAppointmentHandler(IAppointmentRepo repo) {
            this.repo = repo;
        }

        public async override Task Execute(DeleteAppointmentCommand input, User? user) {
            var app = (await repo.FindById(input.Id)) ?? throw new EntityNotFoundException();

            if (!app.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await repo.Delete(app);
        }
    }
}