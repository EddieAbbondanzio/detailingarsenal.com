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
    [Route("product-catalog/reviews")]
    public class ReviewsController : ControllerBase {
        IMediator mediator;

        public ReviewsController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateCommand create) {
            var id = await mediator.Dispatch<ReviewCreateCommand, Guid>(
                create,
                User
            );

            var r = await mediator.Dispatch<GetReviewByIdQuery, ReviewReadModel>(new(id));
            return Ok(r);
        }
    }
}