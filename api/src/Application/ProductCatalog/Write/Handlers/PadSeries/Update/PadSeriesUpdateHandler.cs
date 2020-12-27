using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "update", Scope = "pad-series")]
    public class PadSeriesUpdateHandler : ActionHandler<PadSeriesUpdateCommand> {
        IPadSeriesRepo repo;
        PadSeriesCreateOrUpdateCompositeSpecification spec;

        public PadSeriesUpdateHandler(IPadSeriesRepo repo, PadSeriesCreateOrUpdateCompositeSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task Execute(PadSeriesUpdateCommand command, User? user) {
            var series = await repo.FindById(command.Id) ?? throw new EntityNotFoundException();

            series.Name = command.Name;
            series.BrandId = command.BrandId;
            series.Material = command.Material;
            series.Texture = command.Texture;
            series.Colors = command.Colors;
            series.Sizes = command.Sizes;

            foreach (var color in series.Colors) {
                if (color.Image != null) {
                    color.Image.Parent = new ImageParentReference(color.Id, ImageParentType.PadColor);
                }
            }

            await spec.CheckAndThrow(series);
            await repo.Update(series);
        }
    }
}