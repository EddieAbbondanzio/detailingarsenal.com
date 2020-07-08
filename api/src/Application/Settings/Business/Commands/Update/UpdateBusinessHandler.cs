using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(UpdateBusinessValidator))]
    [Authorization(Action = "update", Scope = "businesses")]
    public class UpdateBusinessHandler : ActionHandler<UpdateBusinessCommand, BusinessView> {
        IBusinessService service;
        private IMapper mapper;

        public UpdateBusinessHandler(IBusinessService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<BusinessView> Execute(UpdateBusinessCommand command, User? user) {
            Business b = await service.GetByUser(user!);

            if (!b.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(b, new BusinessUpdate(
                command.Name,
                command.Address,
                command.Phone
            ));

            return mapper.Map<Business, BusinessView>(b);
        }
    }
}