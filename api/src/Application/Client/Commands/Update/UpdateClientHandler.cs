using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "update", Scope = "clients")]
    public class UpdateClientHandler : ActionHandler<UpdateClientCommand, ClientDto> {
        private IClientRepo repo;
        private IMapper mapper;

        public UpdateClientHandler(IClientRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(UpdateClientCommand input, User? user) {
            var c = (await repo.FindById(input.Id)) ?? throw new EntityNotFoundException();

            if (c.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            c.Name = input.Name;
            c.Phone = input.Phone;
            c.Email = input.Email;

            await repo.Update(c);
            return mapper.Map<Client, ClientDto>(c);
        }
    }
}