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
            var sizes = command.Sizes.Select(s => new PadSize(s.Diameter, s.Thickness)).OrderByDescending(s => s.Diameter).ToList();

            var colors = command.Colors.Select(c => {
                ProcessedImage? image = null;

                if (c.Image != null) {
                    var dataUrlImage = c.Image.Right(); // Crash and burn if Guid passed.
                    image = imageProcessor.Process(dataUrlImage.Name, dataUrlImage.Data);
                }

                var options = c.Options.Select(o => {
                    if (o.PadSizeIndex.HasValue) {
                        return new PadOption(sizes[o.PadSizeIndex.Value].Id, o.PartNumber);
                    } else if (o.PadSizeId.HasValue) {
                        return new PadOption(o.PadSizeId.Value, o.PartNumber);
                    } else {
                        throw new InvalidOperationException($"Pad color {c.Name} has option without pad size id, or pad size index defined.");
                    }
                }).ToList();

                return new PadColor(c.Name, c.Category, c.Material, c.Texture, image, options);
            }).OrderBy(c => c.Name).ToList();


            var series = new PadSeries(
                command.Name,
                command.BrandId,
                command.PolisherTypes,
                sizes,
                colors
            );

            await spec.CheckAndThrow(series);
            await repo.Add(series);
            return series.Id;
        }
    }
}