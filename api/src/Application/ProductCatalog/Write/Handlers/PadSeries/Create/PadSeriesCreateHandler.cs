using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "pad-series")]
    public class PadSeriesCreateHandler : ActionHandler<PadSeriesCreateCommand, Guid> {
        PadSeriesCreateOrUpdateCompositeSpecification spec;
        IPadSeriesRepo repo;

        IImageProcessor imageProcessor;

        public PadSeriesCreateHandler(PadSeriesCreateOrUpdateCompositeSpecification spec, IPadSeriesRepo repo, IImageProcessor imageProcessor) {
            this.spec = spec;
            this.repo = repo;
            this.imageProcessor = imageProcessor;
        }

        public async override Task<Guid> Execute(PadSeriesCreateCommand command, User? user) {
            // var sizes = command.Sizes.Select(s => new PadSize(s.Diameter, s.Thickness)).ToList();

            // var series = new PadSeries(
            //     command.Name,
            //     command.BrandId,
            //     command.Material,
            //     command.Texture,
            //     command.PolisherTypes,
            //     sizes,
            //     command.Colors.Select(c => new PadColor(c.Name, c.Category, c.Image != null ? imageProcessor.Process(c.Image.Name, c.Image.Data) : null,
            //         c.Options.Select(o => {
            //             if (o.PadSizeIndex.HasValue) {
            //                 return new PadOption(sizes[o.PadSizeIndex.Value].Id, o.PartNumber);
            //             } else if (o.PadSizeId.HasValue) {
            //                 return new PadOption(o.PadSizeId.Value, o.PartNumber);
            //             } else {
            //                 throw new InvalidOperationException($"Pad color {c.Name} has option without pad size id, or pad size index defined.");
            //             }
            //         }).ToList()
            //     )).ToList()
            // );

            // foreach (var color in series.Colors) {
            //     if (color.Image != null) {
            //         color.Image.Parent = new ImageParentReference(color.Id, ImageParentType.PadColor);
            //     }
            // }

            // await spec.CheckAndThrow(series);
            // await repo.Add(series);
            // return series.Id;
            return Guid.NewGuid();
        }
    }
}