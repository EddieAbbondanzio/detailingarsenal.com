using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "read", Scope = "clients")]
    public class GetClientsHandler : ActionHandler<GetClientsQuery, List<ClientDto>> {
        IClientService service;
        private IMapper mapper;

        public GetClientsHandler(IClientService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<ClientDto>> Execute(GetClientsQuery input, User? user) {
            List<Client> clients = await service.GetByUser(user!);
            return mapper.Map<List<Client>, List<ClientDto>>(clients);
        }
    }
}