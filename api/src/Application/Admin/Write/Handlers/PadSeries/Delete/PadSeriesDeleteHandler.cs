using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [Authorization(Action = "delete", Scope = "pad-series")]
    public class PadSeriesDeleteHandler : ActionHandler<PadSeriesDeleteCommand> {
        IPadSeriesRepo repo;

        public PadSeriesDeleteHandler(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        public async override Task Execute(PadSeriesDeleteCommand input, User? user) {
            var ps = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await repo.Delete(
                ps
            );
        }
    }
}