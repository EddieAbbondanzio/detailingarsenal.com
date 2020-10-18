using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "update", Scope = "pad-series")]
    public class PadSeriesUpdateHandler : ActionHandler<PadSeriesUpdateCommand, CommandResult> {
        IPadSeriesRepo repo;

        public PadSeriesUpdateHandler(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(PadSeriesUpdateCommand command, User? user) {
            var series = await repo.FindById(command.Id) ?? throw new EntityNotFoundException();

            series.Name = command.Name;
            series.BrandId = command.BrandId;
            series.Sizes = command.Sizes;
            series.Pads = command.Pads.Select(p => new Pad(
                p.Name, p.Category, p.Material, p.Texture, p.PolisherTypes
            )).ToList();

            await repo.Update(series);

            return CommandResult.Success();
        }
    }
}