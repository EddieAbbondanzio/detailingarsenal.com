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
    [Route("product-catalog/pad-series")]
    public class PadSeriesController : ControllerBase {
        private IMediator mediator;

        public PadSeriesController(IMediator mediator) {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            List<PadSeriesReadModel> PadSeriess = await mediator.Dispatch<GetAllPadSeriesQuery, List<PadSeriesReadModel>>(
                new GetAllPadSeriesQuery()
            );

            return Ok(PadSeriess);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PadSeriesCreateRequest create) {
            PadSeriesReadModel PadSeries = await mediator.Dispatch<PadSeriesCreateCommand, PadSeriesReadModel>(
                create,
                User.GetUserId()
            );

            return Ok(PadSeries);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PadSeriesUpdateRequest update) {
            PadSeriesReadModel PadSeries = await mediator.Dispatch<PadSeriesUpdateCommand, PadSeriesReadModel>(
                update,
                User.GetUserId()
            );

            return Ok(PadSeries);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id) {
            await mediator.Dispatch<PadSeriesDeleteCommand>(
                new PadSeriesDeleteCommand(id),
                User.GetUserId()
            );

            return Ok();
        }
    }
}