using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "create", Scope = "clients")]
    public class CreateClientHandler : ActionHandler<CreateClientCommand, ClientDto> {
        IClientService service;
        private IMapper mapper;

        public CreateClientHandler(IClientService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(CreateClientCommand input, User? user) {
            var c = await service.Create(
                new CreateClient(
                    input.Name,
                    input.Phone,
                    input.Email
                ),
                user!
            );

            return mapper.Map<Client, ClientDto>(c);
        }
    }
}