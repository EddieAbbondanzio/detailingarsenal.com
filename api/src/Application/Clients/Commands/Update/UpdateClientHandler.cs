using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "update", Scope = "clients")]
    public class UpdateClientHandler : ActionHandler<UpdateClientCommand, ClientDto> {
        IClientService service;
        private IMapper mapper;

        public UpdateClientHandler(IClientService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(UpdateClientCommand input, User? user) {
            var c = await service.GetById(input.Id);

            if (!c.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(c, new UpdateClient(
                input.Name,
                input.Phone,
                input.Email
            ));

            return mapper.Map<Client, ClientDto>(c);
        }
    }
}