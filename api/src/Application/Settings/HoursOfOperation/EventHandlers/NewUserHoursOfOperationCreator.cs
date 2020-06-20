using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    /// <summary>
    /// When a new user is generated, go ahead and create an empty HoursOfOperation for the user.
    /// </summary>
    public class NewUserHoursOfOperationCreator : IBusEventHandler<NewUserEvent> {
        private IHoursOfOperationRepo repo;

        public NewUserHoursOfOperationCreator(IHoursOfOperationRepo repo) {
            this.repo = repo;
        }

        public async Task Handle(NewUserEvent busEvent) {
            var hours = HoursOfOperation.Create(
                busEvent.User.Id
            );

            // Default to Mon - Fri 8AM to 5PM
            for (int d = 1; d <= 6; d++) {
                hours.Days.Add(
                    HoursOfOperationDay.Create(
                        hours.Id,
                        d,
                        8 * 60,
                        17 * 60
                    )
                );
            }

            await repo.Add(hours);
        }
    }
}