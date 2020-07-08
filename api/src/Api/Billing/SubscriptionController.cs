using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Billing;
using DetailingArsenal.Domain.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Billing {
    [Authorize]
    [ApiController]
    [Route("/billing/subscription")]
    public class SubscriptionController : ControllerBase {
        IMediator mediator;

        public SubscriptionController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserSubscription() {
            var sub = await mediator.Dispatch<GetUserSubscriptionQuery, SubscriptionReadModel>(
                new GetUserSubscriptionQuery(),
                User.GetUserId()
            );

            return Ok(sub);
        }
    }
}