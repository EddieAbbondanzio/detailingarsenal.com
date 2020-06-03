using System;
using System.Threading.Tasks;
using AutoMapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "delete", Scope = "clients")]
    public class DeleteClientHandler : ActionHandler<DeleteClientCommand, ClientDto> {
        private ClientHasNoAppointmentsSpecification specification;
        private IClientRepo repo;
        private IMapper mapper;

        public DeleteClientHandler(ClientHasNoAppointmentsSpecification specification, IClientRepo repo, IMapper mapper) {
            this.specification = specification;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<ClientDto> Execute(DeleteClientCommand input, User? user) {
            var c = (await repo.FindById(input.Id)) ?? throw new EntityNotFoundException();

            if (c.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            await specification.CheckAndThrow(c);

            await repo.Delete(c);
            return mapper.Map<Client, ClientDto>(c);
        }
    }
}