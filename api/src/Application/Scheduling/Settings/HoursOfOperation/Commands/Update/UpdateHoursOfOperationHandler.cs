using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "update", Scope = "hours-of-operations")]
    [Validation(typeof(UpdateHoursOfOperationValidator))]
    public class UpdateHoursOfOperationHandler : ActionHandler<UpdateHoursOfOperationCommand, HoursOfOperationView> {
        private IHoursOfOperationService service;

        public UpdateHoursOfOperationHandler(IHoursOfOperationService service) {
            this.service = service;
        }

        public async override Task<HoursOfOperationView> Execute(UpdateHoursOfOperationCommand input, User? user) {
            var hours = await service.GetById(input.Id);

            if (!hours.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(hours, new HoursOfOperationUpdate(
                input.Days.Select(d => new HoursOfOperationDayUpdate(
                    d.Day,
                    d.Open,
                    d.Close,
                    d.Enabled
                )).ToList()
            ));

            throw new NotImplementedException();
            // return mapper.Map<HoursOfOperation, HoursOfOperationView>(hours);
        }
    }
}