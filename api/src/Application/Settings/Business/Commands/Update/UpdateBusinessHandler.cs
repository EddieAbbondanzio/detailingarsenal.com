using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(UpdateBusinessValidator))]
    [Authorization(Action = "update", Scope = "businesses")]
    public class UpdateBusinessHandler : ActionHandler<UpdateBusinessCommand, BusinessDto> {
        IBusinessService service;
        private IMapper mapper;

        public UpdateBusinessHandler(IBusinessService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<BusinessDto> Execute(UpdateBusinessCommand command, User? user) {
            Business b = await service.GetByUser(user!);

            if (!b.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(b, new UpdateBusiness(
                command.Name,
                command.Address,
                command.Phone
            ));

            return mapper.Map<Business, BusinessDto>(b);
        }
    }
}