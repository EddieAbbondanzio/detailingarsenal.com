using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    /// <summary>
    /// When a new user is generated, go ahead and create an empty HoursOfOperation for the user.
    /// </summary>
    public class NewUserHoursOfOperationCreator : IBusEventHandler<NewUserEvent> {
        private IHoursOfOperationRepo repo;

        public NewUserHoursOfOperationCreator(IHoursOfOperationRepo repo) {
            this.repo = repo; //
        }

        public async Task Handle(NewUserEvent busEvent) {
            var hours = new HoursOfOperation() {
                Id = Guid.NewGuid(),
                UserId = busEvent.User.Id,
            };

            // Default to Mon - Fri 8AM to 5PM
            for (int d = 1; d <= 6; d++) {
                hours.Days.Add(new HoursOfOperationDay() {
                    Id = Guid.NewGuid(),
                    HoursOfOperationId = hours.Id,
                    Day = d,
                    Open = 8 * 60,
                    Close = 17 * 60,
                    Enabled = true
                });
            }

            await repo.Add(hours);
        }
    }
}