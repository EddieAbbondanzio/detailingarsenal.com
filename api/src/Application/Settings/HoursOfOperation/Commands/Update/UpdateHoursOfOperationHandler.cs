using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "update", Scope = "hours-of-operations")]
    [Validation(typeof(UpdateHoursOfOperationValidator))]
    public class UpdateHoursOfOperationHandler : ActionHandler<UpdateHoursOfOperationCommand, HoursOfOperationDto> {
        private IHoursOfOperationService service;
        private IMapper mapper;

        public UpdateHoursOfOperationHandler(IHoursOfOperationService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<HoursOfOperationDto> Execute(UpdateHoursOfOperationCommand input, User? user) {
            var hours = await service.GetById(input.Id);

            if (!hours.IsOwner(user!)) {
                throw new AuthorizationException();
            }

            await service.Update(hours, new UpdateHoursOfOperation(
                input.Days.Select(d => new UpdateHoursOfOperationDay(
                    d.Day,
                    d.Open,
                    d.Close,
                    d.Enabled
                )).ToList()
            ));

            return mapper.Map<HoursOfOperation, HoursOfOperationDto>(hours);
        }
    }
}