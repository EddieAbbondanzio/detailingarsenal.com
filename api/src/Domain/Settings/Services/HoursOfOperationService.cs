using System;
using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    public interface IHoursOfOperationService : IService {
        Task<HoursOfOperation> GetByUser(User user);
        Task<HoursOfOperation> GetById(Guid id);
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