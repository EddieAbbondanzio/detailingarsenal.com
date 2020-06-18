using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization(Action = "update", Scope = "hours-of-operations")]
    public class UpdateHoursOfOperationHandler : ActionHandler<UpdateHoursOfOperationCommand, HoursOfOperationDto> {
        private IHoursOfOperationRepo repo;
        private IMapper mapper;

        public UpdateHoursOfOperationHandler(IHoursOfOperationRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<HoursOfOperationDto> Execute(UpdateHoursOfOperationCommand input, User? user) {
            var hours = await repo.FindById(input.Id) ?? throw new InvalidOperationException();

            if (hours.UserId != user!.Id) {
                throw new AuthorizationException("Unauthorized");
            }

            hours.Days = input.Days.Select(d => HoursOfOperationDay.Create(
                hours.Id,
                d.Day,
                d.Open,
                d.Close,
                d.Enabled
            )).ToList();

            await repo.Update(hours);
            return mapper.Map<HoursOfOperation, HoursOfOperationDto>(hours);
        }
    }
}