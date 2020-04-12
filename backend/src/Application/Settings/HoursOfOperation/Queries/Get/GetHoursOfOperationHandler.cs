using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class GetHoursOfOperationHandler : ActionHandler<GetHoursOfOperationQuery, HoursOfOperationDto> {
        private IHoursOfOperationRepo repo;
        private IMapper mapper;

        public GetHoursOfOperationHandler(IHoursOfOperationRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        protected async override Task<HoursOfOperationDto> Execute(GetHoursOfOperationQuery query, User? user) {
            var hours = await repo.FindForUser(user!);
            return mapper.Map<HoursOfOperation, HoursOfOperationDto>(hours);
        }
    }
}