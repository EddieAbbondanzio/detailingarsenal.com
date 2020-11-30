using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Scheduling.Billing;
using DetailingArsenal.Domain.Scheduling.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Scheduling.Billing {
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
            var plans = await mediator.Dispatch<GetAllSubscriptionPlansQuery, List<SubscriptionPlanReadModel>>(
                new GetAllSubscriptionPlansQuery(),
                User
            );

            return Ok(plans);
        }

        [HttpGet("default")]
        public async Task<IActionResult> GetDefault() {
            var plan = await mediator.Dispatch<GetDefaultSubscriptionPlanQuery, SubscriptionPlanReadModel>();// Anon is allowed

            return Ok(plan);
        }

        /// <summary>
        /// Update the description and role id of a subscription plan.
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SubscriptionPlanUpdateRequest body) {
            await mediator.Dispatch<SubscriptionPlanUpdateCommand>(
                new(id, body.Description, body.RoleId), User
            );

            var plan = await mediator.Dispatch<GetByIdSubscriptionPlanQuery, SubscriptionPlanReadModel?>(
                new(id), User
            );

            return Ok(plan);
        }

        /// <summary>
        /// Endpoint to refresh the cache of subscription plans by querying Auth0.
        /// </summary>
        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh() {
            await mediator.Dispatch<RefreshSubscriptionPlansCommand>(User);

            var plans = await mediator.Dispatch<GetAllSubscriptionPlansQuery, List<SubscriptionPlanReadModel>>(User);
            return Ok(plans);
        }
    }
}