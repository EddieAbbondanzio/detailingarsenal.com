using System;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.ProductCatalog {
    public interface IPadSeriesService : IService {
        Task<PadSeries> GetById(Guid id);
        Task<PadSeries> Create(PadSeriesCreate create, User user);
        Task<PadSeries> Update(PadSeries series, PadSeriesUpdate update, User user);
        Task Delete(PadSeries series, User user);
    }

    public class PadSeriesService : IPadSeriesService {
        IPadSeriesRepo repo;

        public PadSeriesService(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        public async Task<PadSeries> GetById(Guid id) {
            var series = await repo.FindById(id);
            return series ?? throw new EntityNotFoundException();
        }

        public async Task<PadSeries> Create(PadSeriesCreate create, User user) {
            var pads = create.Pads.Select(p => new Pad(p.Category, p.Name, p.Image)).ToList();

            var series = new PadSeries(
                create.Name,
                create.BrandId,
                pads
            );

            await repo.Add(series);
            return series;
        }

        public async Task<PadSeries> Update(PadSeries series, PadSeriesUpdate update, User user) {
            series.Name = update.Name;
            series.BrandId = update.BrandId;
            series.Pads = update.Pads.Select(p => new Pad(p.Category, p.Name, p.Image)).ToList();

            await repo.Update(series);
            return series;
        }

        public async Task Delete(PadSeries series, User user) {
            await repo.Delete(series);
        }
    }
}