using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "create", Scope = "clients")]
    public class CreateClientHandler : ActionHandler<CreateClientCommand, ClientView> {
        IClientService service;
        private IMapper mapper;

        public CreateClientHandler(IClientService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
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

            return mapper.Map<Client, ClientView>(c);
        }
    }
}