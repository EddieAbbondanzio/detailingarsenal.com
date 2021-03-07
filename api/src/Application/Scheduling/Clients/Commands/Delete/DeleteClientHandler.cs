using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "delete", Scope = "clients")]
    [DependencyInjection]
    public class DeleteClientHandler : ActionHandler<DeleteClientCommand, ClientView> {
        IClientService service;

        public DeleteClientHandler(IClientService service) {
            this.service = service;
        }

        public async override Task<ClientView> Execute(DeleteClientCommand input, User? user) {
            var c = await service.GetById(input.Id);

            if (!c.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(c);
            throw new NotImplementedException();
            // return mapper.Map<Client, ClientView>(c);
        }
    }
}