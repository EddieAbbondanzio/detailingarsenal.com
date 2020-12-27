using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DetailingArsenal.Application.Settings;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Application.ProductCatalog;
using System.Drawing;
using System.IO;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Api.ProductCatalog {
    [Authorize]
    [ApiController]
    [Route("product-catalog/pad-series")]
    public class PadSeriesController : ControllerBase {
        IMediator mediator;
        IImageProcessor imageProcessor;

        public PadSeriesController(IMediator mediator, IImageProcessor imageProcessor) {
            this.mediator = mediator;
            this.imageProcessor = imageProcessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<PadSeriesReadModel> pads = await mediator.Dispatch<GetAllPadSeriesQuery, List<PadSeriesReadModel>>();
            return Ok(pads);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PadSeriesCreateRequest create) {
            var sizes = create.Sizes.Select(
                s => new PadSize(
                    new Measurement(s.Diameter.Amount, MeasurementUnitUtils.Parse(s.Diameter.Unit)),
                    s.Thickness != null ? new Measurement(s.Thickness.Amount, MeasurementUnitUtils.Parse(s.Thickness.Unit)) : null
            )).ToList();

            var id = await mediator.Dispatch<PadSeriesCreateCommand, Guid>(
                new PadSeriesCreateCommand(
                    create.Name,
                    create.BrandId,
                    PadTextureUtils.Parse(create.Texture),
                    PadMaterialUtils.Parse(create.Material),
                    create.PolisherTypes.Select(pt => PolisherTypeUtils.Parse(pt)).ToList(),
                    sizes,
                    create.Colors.Select(
                        c => new PadColor(
                            c.Name,
                            PadCategoryUtils.Parse(c.Category),
                            c.Image != null ? imageProcessor.Process(c.Image.Name, c.Image.Data) : null,
                            c.Options.Select(o => new PadOption(sizes[o.PadSizeIndex].Id, o.PartNumber)).ToList()
                        )
                    ).ToList()),
                User
            );

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id));

            return Ok(ps);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PadSeriesUpdateRequest update) {
            await mediator.Dispatch<PadSeriesUpdateCommand>(
                new PadSeriesUpdateCommand(
                    id,
                    update.Name,
                    update.BrandId,
                    PadTextureUtils.Parse(update.Texture),
                    PadMaterialUtils.Parse(update.Material),
                    update.PolisherTypes.Select(pt => PolisherTypeUtils.Parse(pt)).ToList(),
                    update.Sizes.Select(
                        s => new PadSize(
                            new Measurement(s.Diameter.Amount, MeasurementUnitUtils.Parse(s.Diameter.Unit)),
                            s.Thickness != null ? new Measurement(s.Thickness.Amount, MeasurementUnitUtils.Parse(s.Thickness.Unit)) : null
                        )).ToList(),
                    update.Colors.Select(
                        c => new PadColor(
                            c.Name,
                            PadCategoryUtils.Parse(c.Category),
                            c.Image != null ? imageProcessor.Process(c.Image.Name, c.Image.Data) : null,
                            c.Options.Select(o => new PadOption(o.PadSizeId, o.PartNumber)).ToList()
                        )
                    ).ToList()),
                User
            );

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(new(id));

            return Ok(ps);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            await mediator.Dispatch<PadSeriesDeleteCommand>(new(id), User);
            return Ok();
        }
    }
}