using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class DeleteAppointmentHandler : ActionHandler<DeleteAppointmentCommand> {
        private IAppointmentRepo repo;

        public DeleteAppointmentHandler(IAppointmentRepo repo) {
            this.repo = repo;
        }

        protected async override Task Execute(DeleteAppointmentCommand input, User? user) {
            var app = (await repo.FindById(input.Id)) ?? throw new EntityNotFoundException();

            if (app.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await repo.Delete(app);
        }
    }
}