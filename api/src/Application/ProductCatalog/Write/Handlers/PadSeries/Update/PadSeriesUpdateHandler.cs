using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Validation(typeof(PadSeriesUpdateValidator))]
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
            var origColors = series.Pads;


            series.Name = command.Name;
            series.BrandId = command.BrandId;

            series.Sizes = UpdatePadSizes(series.Sizes, command.Sizes);
            series.Pads = UpdatePads(series.Sizes, series.Pads, command.Pads);


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

            return sizes.OrderByDescending(s => s.Diameter).ToList();
        }

        public List<Pad> UpdatePads(List<PadSize> sizes, List<Pad> existing, List<PadCreateOrUpdate> updates) {
            var colors = new List<Pad>();

            // Update any existing pad colors
            foreach (var pad in existing) {
                var update = updates.Find(c => c.Id == pad.Id);

                // If no update exists, it was deleted.
                if (update == null) {
                    continue;
                }

                pad.Name = update.Name;
                pad.Category = update.Category;
                pad.Material = update.Material;
                pad.Texture = update.Texture;
                pad.Color = update.Color;
                pad.HasCenterHole = update.HasCenterHole;
                pad.Image = update.Image?.Match(id => pad.Image, image => imageProcessor.Process(image.Name, image.Data));


                pad.Options.Clear();
                foreach (var option in update.Options) {
                    pad.Options.Add(new PadOption(sizes[option.PadSizeIndex].Id, option.PartNumbers));
                }

                colors.Add(pad);
            }

            // Add new
            var newColors = updates.Where(u => u.Id == null);
            foreach (var update in newColors) {

                var color = new Pad(
                    update.Name,
                    update.Category,
                    update.Material,
                    update.Texture,
                    update.Color,
                    update.HasCenterHole,
                    update.Image?.Match(
                        id => existing.Find(c => c.Image?.Id == id)?.Image,
                        image => imageProcessor.Process(image.Name, image.Data)
                    ),
                    update.Options.Select(option => new PadOption(sizes[option.PadSizeIndex].Id, option.PartNumbers)).ToList()
                );

                // Sort options
                color.Options = color.Options.OrderByDescending(o => sizes.Find(s => s.Id == o.PadSizeId)!.Diameter).ToList();

                colors.Add(color);
            }

            return colors.OrderBy(c => c.Name).ToList();
        }
    }
}