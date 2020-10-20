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

namespace DetailingArsenal.Api.ProductCatalog {
    [Authorize]
    [ApiController]
    [Route("product-catalog/pad")]
    public class PadSeriesController : ControllerBase {
        private IMediator mediator;

        public PadSeriesController(IMediator mediator) {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<PadSeriesReadModel> pads = await mediator.Dispatch<GetAllPadSeriesQuery, List<PadSeriesReadModel>>(
                new GetAllPadSeriesQuery()
            );

            return Ok(pads);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PadSeriesCreateRequest create) {
            var res = await mediator.Dispatch<PadSeriesCreateCommand, CommandResult>(
                new PadSeriesCreateCommand(
                    create.Name,
                    create.BrandId,
                    create.Sizes.Select(s => new PadSeriesSize(s.Diameter, s.Thickness, s.PartNumber)).ToList(),
                    create.Pads.Select(p => p.ToReal()).ToList()
                ),
                User.GetUserId()
            );

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(
                new GetPadSeriesByIdQuery(res.Data.Id)
            );

            return Ok(ps);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PadSeriesUpdateRequest update) {
            var res = await mediator.Dispatch<PadSeriesUpdateCommand, CommandResult>(
                new PadSeriesUpdateCommand(
                    id,
                    update.Name,
                    update.BrandId,
                    update.Sizes,
                    update.Pads.Select(p => p.ToReal()).ToList()
                ),
                User.GetUserId()
            );

            var ps = await mediator.Dispatch<GetPadSeriesByIdQuery, PadSeriesReadModel>(
                new GetPadSeriesByIdQuery(id)
            );

            return Ok(ps);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            var res = await mediator.Dispatch<PadSeriesDeleteCommand, CommandResult>(
                new PadSeriesDeleteCommand(id),
                User.GetUserId()
            );

            return Ok();
        }
    }
}