using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "delete", Scope = "pads")]
    public class PadDeleteHandler : ActionHandler<PadDeleteCommand, CommandResult> {
        IPadSeriesRepo repo;

        public PadDeleteHandler(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(PadDeleteCommand input, User? user) {
            var ps = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await repo.Delete(
                ps
            );

            return CommandResult.Success();
        }
    }
}