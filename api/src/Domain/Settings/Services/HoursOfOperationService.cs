using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IHoursOfOperationService : IService {
        Task<HoursOfOperation> GetByUser(User user);
        Task<HoursOfOperation> GetById(Guid id);
        Task<HoursOfOperation> CreateDefault(User user);
        Task Update(HoursOfOperation hours, UpdateHoursOfOperation update);
    }

    public class HoursOfOperationService : IHoursOfOperationService {
        private IHoursOfOperationRepo repo;

        public HoursOfOperationService(IHoursOfOperationRepo repo) {
            this.repo = repo;
        }

        public async Task<HoursOfOperation> GetByUser(User user) {
            return await repo.FindForUser(user) ?? throw new EntityNotFoundException();
        }

        public async Task<HoursOfOperation> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<HoursOfOperation> CreateDefault(User user) {
            var hours = HoursOfOperation.Create(
                user.Id
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
            return hours;
        }

        public async Task Update(HoursOfOperation hours, UpdateHoursOfOperation update) {
            hours.Days = update.Days.Select(d => HoursOfOperationDay.Create(
                hours.Id,
                d.Day,
                d.Open,
                d.Open,
                d.Enabled
            )).ToList();

            await repo.Update(hours);
        }

    }
}