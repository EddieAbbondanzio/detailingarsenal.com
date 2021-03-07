using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "update", Scope = "clients")]
    [DependencyInjection]
    public class UpdateClientHandler : ActionHandler<UpdateClientCommand, ClientView> {
        IClientService service;

        public UpdateClientHandler(IClientService service) {
            this.service = service;
        }

        public async override Task<ClientView> Execute(UpdateClientCommand input, User? user) {
            var c = await service.GetById(input.Id);

            if (!c.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(c, new ClientUpdate(
                input.Name,
                input.Phone,
                input.Email
            ));

            throw new NotImplementedException();
            // return mapper.Map<Client, ClientView>(c);
        }
    }
}