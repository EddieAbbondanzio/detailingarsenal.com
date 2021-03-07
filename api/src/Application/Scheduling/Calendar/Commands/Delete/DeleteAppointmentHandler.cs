using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Calendar {
    [Authorization(Action = "delete", Scope = "appointments")]
    [DependencyInjection]
    public class DeleteAppointmentHandler : ActionHandler<DeleteAppointmentCommand> {
        IAppointmentService service;

        public DeleteAppointmentHandler(IAppointmentService service) {
            this.service = service;
        }

        public async override Task Execute(DeleteAppointmentCommand input, User? user) {
            var app = await service.GetById(input.Id);

            if (!app.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(app);
        }
    }
}