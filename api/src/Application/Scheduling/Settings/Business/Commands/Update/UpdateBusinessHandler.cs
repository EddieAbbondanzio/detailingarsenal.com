using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;
using System;

namespace DetailingArsenal.Application.Settings {
    [Validation(typeof(UpdateBusinessValidator))]
    [Authorization(Action = "update", Scope = "businesses")]
    public class UpdateBusinessHandler : ActionHandler<UpdateBusinessCommand, BusinessView> {
        IBusinessService service;

        public UpdateBusinessHandler(IBusinessService service) {
            this.service = service;
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

            throw new NotImplementedException();
            // return mapper.Map<Business, BusinessView>(b);
        }
    }
}