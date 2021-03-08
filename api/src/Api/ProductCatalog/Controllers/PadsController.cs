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
    [ApiController]
    [Route("product-catalog/pads")]
    public class PadsController : ControllerBase {
        IMediator mediator;

        public PadsController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilter() {
            var filter = await mediator.Dispatch<GetPadSeriesFilterQuery, PadFilterReadModel>();
            return Ok(filter);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var pads = await mediator.Dispatch<GetAllPadsQuery, PagedCollection<PadReadModel>>();
            return Ok(pads);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            var pad = await mediator.Dispatch<GetPadByIdQuery, PadReadModel?>(new(id));
            return Ok(pad);
        }

        [HttpGet("{id}/sizes")]
        public async Task<IActionResult> GetPadSizes(Guid id) {
            var sizes = await mediator.Dispatch<GetAllSizesForPadQuery, List<PadSizeReadModel>>(
                new(id),
                User
            );

            return Ok(sizes);

        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviews(Guid id) {
            List<ReviewReadModel> reviews = await mediator.Dispatch<GetAllReviewsForPadQuery, List<ReviewReadModel>>(
                new(id),
                User
            );

            return Ok(reviews);
        }
    }
}