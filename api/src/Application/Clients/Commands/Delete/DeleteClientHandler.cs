using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "delete", Scope = "clients")]
    public class DeleteClientHandler : ActionHandler<DeleteClientCommand, ClientDto> {
        IClientService service;
        private IMapper mapper;

        public DeleteClientHandler(IClientService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(DeleteClientCommand input, User? user) {
            var c = await service.GetById(input.Id);

            if (!c.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(c);
            return mapper.Map<Client, ClientDto>(c);
        }
    }
}