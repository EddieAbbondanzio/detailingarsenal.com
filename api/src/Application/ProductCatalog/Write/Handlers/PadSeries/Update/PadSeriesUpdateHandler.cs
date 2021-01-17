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
        IImageProcessor imageProcessor;

        public PadSeriesUpdateHandler(IPadSeriesRepo repo, PadSeriesCreateOrUpdateCompositeSpecification spec, IImageProcessor imageProcessor) {
            this.repo = repo;
            this.spec = spec;
            this.imageProcessor = imageProcessor;
        }

        public async override Task Execute(PadSeriesUpdateCommand command, User? user) {
            var series = await repo.FindById(command.Id) ?? throw new EntityNotFoundException();
            var origColors = series.Colors;

            series.Name = command.Name;
            series.BrandId = command.BrandId;
            series.Material = command.Material;
            series.Texture = command.Texture;

            series.Sizes = UpdatePadSizes(series.Sizes, command.Sizes);
            series.Colors = UpdatePadColors(series.Colors, command.Colors);


            await spec.CheckAndThrow(series);
            await repo.Update(series);
        }

        public List<PadSize> UpdatePadSizes(List<PadSize> existing, List<PadSizeCreateOrUpdate> updates) {
            var sizes = new List<PadSize>();

            // Update any existing pad sizes
            foreach (var size in existing) {
                var update = updates.Find(s => s.Id == size.Id);

                // If no update with the pad series exists, it was deleted.
                if (update == null)
                    continue;

                size.Diameter = update.Diameter;
                size.Thickness = update.Thickness;

                // Add it to the new list.
                sizes.Add(size);
            }

            // Add any new sizes
            var newSizes = updates.Where(s => s.Id == null);
            foreach (var size in newSizes) {
                sizes.Add(new PadSize(size.Diameter, size.Thickness));
            }

            return sizes;
        }

        public List<PadColor> UpdatePadColors(List<PadColor> existing, List<PadColorCreateOrUpdate> updates) {
            var colors = new List<PadColor>();

            // Update any existing pad colors
            foreach (var color in existing) {
                var update = updates.Find(c => c.Id == color.Id);

                // If no update exists, it was deleted.
                if (update == null) {
                    continue;
                }

                color.Name = update.Name;
                color.Category = update.Category;

                color.Image = update.Image?.Match(id => color.Image, image => imageProcessor.Process(image.Name, image.Data));


                color.Options.Clear();
                foreach (var option in update.Options) {
                    color.Options.Add(new PadOption(option.PadSizeId!.Value, option.PartNumber));
                }

                colors.Add(color);
            }

            // Add new
            var newColors = updates.Where(u => u.Id == null);
            foreach (var update in newColors) {

                var color = new PadColor(
                    update.Name,
                    update.Category,
                    update.Image?.Match(
                        id => existing.Find(c => c.Image?.Id == id)?.Image,
                        image => imageProcessor.Process(image.Name, image.Data)
                    ),
                    update.Options.Select(option => new PadOption(option.PadSizeId!.Value, option.PartNumber)).ToList()
                );

                colors.Add(color);
            }

            return colors;
        }
    }
}