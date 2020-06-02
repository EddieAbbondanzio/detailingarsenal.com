using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class CreateClientHandler : ActionHandler<CreateClientCommand, ClientDto> {
        private IClientRepo repo;
        private IMapper mapper;

        public CreateClientHandler(IClientRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(CreateClientCommand input, User? user) {
            var c = new Client() {
                Id = Guid.NewGuid(),
                UserId = user!.Id,
                Name = input.Name,
                Phone = input.Phone,
                Email = input.Email
            };

            await repo.Add(c);
            return mapper.Map<Client, ClientDto>(c);
        }
    }
}