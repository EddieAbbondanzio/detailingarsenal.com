using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "delete", Scope = "services")]
    [DependencyInjection]
    public class DeleteServiceHandler : ActionHandler<DeleteServiceCommand> {
        private IServiceService service;

        public DeleteServiceHandler(IServiceService service) {
            this.service = service;
        }

        public async override Task Execute(DeleteServiceCommand input, User? user) {
            var s = await this.service.GetById(input.Id);

            if (!s.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(s);
        }
    }
}