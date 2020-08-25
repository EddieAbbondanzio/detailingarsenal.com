using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Billing {
    [Route("/billing/subscription-plan")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase {
        private IMediator mediator;

        public SubscriptionPlanController(IMediator mediator) {
            this.mediator = mediator;
        }


        /// <summary>
        /// Get a list of all the subscription plans.
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get() {
            var plans = await mediator.Dispatch<GetSubscriptionPlansQuery, List<SubscriptionPlanView>>(
                new GetSubscriptionPlansQuery(),
                User.GetUserId()
            );

            return Ok(plans);
        }

        [HttpGet("default")]
        public async Task<IActionResult> GetDefault() {
            var plan = await mediator.Dispatch<GetDefaultSubscriptionPlanQuery, SubscriptionPlanView>(
                new GetDefaultSubscriptionPlanQuery()
            // Anon is allowed
            );

            return Ok(plan);
        }

        /// <summary>
        /// Endpoint to refresh the cache of subscription plans by querying Auth0.
        /// </summary>
        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh() {
            var plans = await mediator.Dispatch<RefreshSubscriptionPlansCommand, List<SubscriptionPlanView>>(
                new RefreshSubscriptionPlansCommand(),
                User.GetUserId()
            );

            return Ok(plans);
        }
    }
}