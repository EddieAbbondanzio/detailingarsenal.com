using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "pads")]
    public class PadSeriesCreateHandler : ActionHandler<PadCreateCommand, CommandResult> {
        IPadSeriesRepo repo;

        public PadSeriesCreateHandler(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(PadCreateCommand command, User? user) {
            var pads = command.Pads.Select(p => new Pad(p.Category, p.Name, p.Image)).ToList();

            var series = new PadSeries(
                command.Name,
                command.BrandId,
                pads
            );

            await repo.Add(series);

            return CommandResult.Insert(series.Id);
        }
    }
}