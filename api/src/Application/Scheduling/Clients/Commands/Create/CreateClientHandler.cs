using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "create", Scope = "clients")]
    [DependencyInjection]
    public class CreateClientHandler : ActionHandler<CreateClientCommand, ClientView> {
        IClientService service;

        public CreateClientHandler(IClientService service) {
            this.service = service;
        }

        public async override Task<ClientView> Execute(CreateClientCommand input, User? user) {
            var c = await service.Create(
                new ClientCreate(
                    input.Name,
                    input.Phone,
                    input.Email
                ),
                user!
            );

            throw new NotImplementedException();
        }
    }
}