using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "delete", Scope = "clients")]
    public class DeleteClientHandler : ActionHandler<DeleteClientCommand, ClientView> {
        IClientService service;
        private IMapper mapper;

        public DeleteClientHandler(IClientService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<ClientView> Execute(DeleteClientCommand input, User? user) {
            var c = await service.GetById(input.Id);

            if (!c.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Delete(c);
            return mapper.Map<Client, ClientView>(c);
        }
    }
}