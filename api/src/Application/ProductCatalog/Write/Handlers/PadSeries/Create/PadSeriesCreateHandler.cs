using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "pad-series")]
    public class PadSeriesCreateHandler : ActionHandler<PadSeriesCreateCommand, Guid> {
        PadSizeDiameterUniqueSpecification spec;
        IPadSeriesRepo repo;

        public PadSeriesCreateHandler(PadSizeDiameterUniqueSpecification spec, IPadSeriesRepo repo) {
            this.spec = spec;
            this.repo = repo;
        }

        public async override Task<Guid> Execute(PadSeriesCreateCommand command, User? user) {
            var series = new PadSeries(
                command.Name,
                command.BrandId,
                command.Material,
                command.Texture,
                command.PolisherTypes,
                command.Sizes,
                command.Colors
            );

            await spec.CheckAndThrow(series);
            await repo.Add(series);
            return series.Id;
        }
    }
}