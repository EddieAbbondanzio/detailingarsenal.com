using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Settings {
    [Authorization(Action = "read", Scope = "hours-of-operations")]
    public class GetHoursOfOperationHandler : ActionHandler<GetHoursOfOperationQuery, HoursOfOperationDto> {
        private IHoursOfOperationService service;
        private IMapper mapper;

        public GetHoursOfOperationHandler(IHoursOfOperationService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<HoursOfOperationDto> Execute(GetHoursOfOperationQuery query, User? user) {
            var hours = await service.GetByUser(user!);
            return mapper.Map<HoursOfOperation, HoursOfOperationDto>(hours);
        }
    }
}