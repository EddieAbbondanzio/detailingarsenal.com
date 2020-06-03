using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "read", Scope = "hours-of-operations")]
    public class GetHoursOfOperationHandler : ActionHandler<GetHoursOfOperationQuery, HoursOfOperationDto> {
        private IHoursOfOperationRepo repo;
        private IMapper mapper;

        public GetHoursOfOperationHandler(IHoursOfOperationRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<HoursOfOperationDto> Execute(GetHoursOfOperationQuery query, User? user) {
            var hours = await repo.FindForUser(user!);
            return mapper.Map<HoursOfOperation, HoursOfOperationDto>(hours);
        }
    }
}