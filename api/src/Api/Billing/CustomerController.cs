using System;
using System.IO;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Billing;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace DetailingArsenal.Api.Billing {
    [Authorize]
    [ApiController]
    [Route("/billing/customer")]
    public class CustomerController : ControllerBase {
        IMediator mediator;
        IDomainEventPublisher eventPublisher;
        ISubscriptionConfig config;

        public CustomerController(IMediator mediator, IDomainEventPublisher eventPublisher, ISubscriptionConfig config) {
            this.mediator = mediator;
            this.eventPublisher = eventPublisher;
            this.config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer() {
            var sub = await mediator.Dispatch<GetCustomerQuery, CustomerReadModel>(
                new GetCustomerQuery(),
                User.GetUserId()
            );

            return Ok(sub);
        }

        [HttpGet("subscription/trial-will-end")]
        public async Task<IActionResult> TrialWillEnd() {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], config.WebhookSecret);

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CustomerSubscriptionTrialWillEnd) {
                    var subscription = stripeEvent.Data.Object as Stripe.Subscription ?? throw new NullReferenceException();

                    await eventPublisher.Dispatch(new CustomerTrialWillEndSoon(
                        subscription.CustomerId
                    ));

                    return Ok(420);
                } else {
                    return Ok();
                }
            } catch (StripeException) {
                return BadRequest();
            }
        }
    }
}