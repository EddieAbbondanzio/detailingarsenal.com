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

        [HttpGet]
        public async Task<IActionResult> GetForPad(Guid padId) {
            List<ReviewReadModel> reviews = await mediator.Dispatch<GetAllReviewsForPadQuery, List<ReviewReadModel>>(
                new GetAllReviewsForPadQuery(padId)
            );

            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateRequest create) {
            var r = await mediator.Dispatch<ReviewCreateCommand, ReviewReadModel>(
                new ReviewCreateCommand(
                    create.PadId, create.Stars, create.Cut, create.Finish, create.Title, create.Body
                )
            );

            return Ok(r);
        }
    }
}