using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "pad-series")]
    public class PadSeriesCreateHandler : ActionHandler<PadSeriesCreateCommand, CommandResult> {
        IPadSeriesRepo repo;

        public PadSeriesCreateHandler(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(PadSeriesCreateCommand command, User? user) {
            var pads = command.Pads.Select(p => new Pad(
                p.Name, p.Category, p.Material, p.Texture, p.PolisherTypes
            )).ToList();

            var series = new PadSeries(
                command.Name,
                command.BrandId,
                command.Sizes,
                pads
            );

            await repo.Add(series);

            return CommandResult.Insert(series.Id);
        }
    }
}