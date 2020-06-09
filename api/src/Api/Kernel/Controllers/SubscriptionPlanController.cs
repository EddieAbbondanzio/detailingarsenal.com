using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("/billing/subscription-plan/")]
[ApiController]
public class SubscriptionPlanController : ControllerBase {
    private IMediator mediator;

    public SubscriptionPlanController(IMediator mediator) {
        this.mediator = mediator;
    }


    /// <summary>
    /// Get a list of all the subscription plans.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get() {
        var plans = await mediator.Dispatch<GetSubscriptionPlansQuery, List<SubscriptionPlan>>(
            new GetSubscriptionPlansQuery(),
            User.GetUserId()
        );

        return Ok(plans);
    }

    /// <summary>
    /// Endpoint to refresh the cache of subscription plans by querying Auth0.
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh() {
        var plans = await mediator.Dispatch<RefreshSubscriptionPlansCommand, List<SubscriptionPlan>>(
            new RefreshSubscriptionPlansCommand(),
            User.GetUserId()
        );

        return Ok(plans);
    }
}