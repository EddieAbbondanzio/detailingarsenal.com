using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IHoursOfOperationService : IService {
        Task<HoursOfOperation> GetByUser(User user);
        Task<HoursOfOperation> GetById(Guid id);
        Task<HoursOfOperation> GetOrCreateForUser(User user);
        Task<HoursOfOperation> CreateDefault(User user);
        Task Update(HoursOfOperation hours, UpdateHoursOfOperation update);
        Task Delete(HoursOfOperation hours);
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

        public async Task<HoursOfOperation> GetOrCreateForUser(User user) {
            var hours = await repo.FindForUser(user);

            if (hours == null) {
                hours = HoursOfOperation.Create(user.Id);
                await repo.Add(hours);
            }

            return hours;
        }


        public async Task<HoursOfOperation> CreateDefault(User user) {
            var hours = HoursOfOperation.Create(
                user.Id
            );

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

        public async Task Delete(HoursOfOperation hours) => await repo.Delete(hours);
    }
}