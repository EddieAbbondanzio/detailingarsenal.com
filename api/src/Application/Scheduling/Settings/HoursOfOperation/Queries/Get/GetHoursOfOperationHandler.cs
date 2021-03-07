using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "hours-of-operations")]
    [DependencyInjection]
    public class GetHoursOfOperationHandler : ActionHandler<GetHoursOfOperationQuery, HoursOfOperationView> {
        private IHoursOfOperationService service;

        public GetHoursOfOperationHandler(IHoursOfOperationService service) {
            this.service = service;
        }

        public async override Task<HoursOfOperationView> Execute(GetHoursOfOperationQuery query, User? user) {
            var hours = await service.GetOrCreateForUser(user!);
            throw new NotImplementedException();
            // return mapper.Map<HoursOfOperation, HoursOfOperationView>(hours);
        }
    }
}