using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "create", Scope = "clients")]
    public class CreateClientHandler : ActionHandler<CreateClientCommand, ClientDto> {
        private IClientRepo repo;
        private IMapper mapper;

        public CreateClientHandler(IClientRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(CreateClientCommand input, User? user) {
            var c = Client.Create(
                user!.Id,
                input.Name,
                input.Phone,
                input.Email
            );

            await repo.Add(c);
            return mapper.Map<Client, ClientDto>(c);
        }
    }
}