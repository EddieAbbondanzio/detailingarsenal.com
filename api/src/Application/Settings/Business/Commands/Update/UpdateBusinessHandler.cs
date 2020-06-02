using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Validation(typeof(UpdateBusinessValidator))]
    public class UpdateBusinessHandler : ActionHandler<UpdateBusinessCommand, BusinessDto> {
        private IBusinessRepo repo;
        private IMapper mapper;

        public UpdateBusinessHandler(IBusinessRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<BusinessDto> Execute(UpdateBusinessCommand command, User? user) {
            Business b = (await repo.FindById(command.Id)) ?? throw new EntityNotFoundException();

            if (b.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            b.Name = command.Name;
            b.Address = command.Address;
            b.Phone = command.Phone;

            await repo.Update(b);
            return mapper.Map<Business, BusinessDto>(b);
        }
    }
}