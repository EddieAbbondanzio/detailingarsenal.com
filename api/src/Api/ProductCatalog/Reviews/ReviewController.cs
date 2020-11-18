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
    [Route("product-catalog/review")]
    public class ReviewController : ControllerBase {
        IMediator mediator;

        public ReviewController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("pad/{id}")]
        public async Task<IActionResult> GetForPad(Guid id) {
            List<ReviewReadModel> reviews = await mediator.Dispatch<GetAllReviewsForPadQuery, List<ReviewReadModel>>(
                new GetAllReviewsForPadQuery(id)
            );

            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateRequest create) {
            var res = await mediator.Dispatch<ReviewCreateCommand, CommandResult>(
                new ReviewCreateCommand(
                    create.PadId, create.Stars, create.Cut, create.Finish, create.Title, create.Body
                )
            );

            var r = await mediator.Dispatch<GetReviewByIdQuery, ReviewReadModel>(new GetReviewByIdQuery(res.Data.Id));
            return Ok(r);
        }
    }
}