using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "read", Scope = "clients")]
    public class GetClientsHandler : ActionHandler<GetClientsQuery, List<ClientDto>> {
        private IClientRepo repo;
        private IMapper mapper;

        public GetClientsHandler(IClientRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<ClientDto>> Execute(GetClientsQuery input, User? user) {
            List<Client> clients = await repo.FindByUser(user!);
            return mapper.Map<List<Client>, List<ClientDto>>(clients);
        }
    }
}