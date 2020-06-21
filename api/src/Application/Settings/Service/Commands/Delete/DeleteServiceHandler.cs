using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "delete", Scope = "services")]
    public class DeleteServiceHandler : ActionHandler<DeleteServiceCommand> {
        private IServiceService service;

        public DeleteServiceHandler(IServiceService service) {
            this.service = service;
        }

        public async override Task Execute(DeleteServiceCommand input, User? user) {
            var s = await this.service.FindById(input.Id) ?? throw new EntityNotFoundException();

            if (!s.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(s);
        }
    }
}