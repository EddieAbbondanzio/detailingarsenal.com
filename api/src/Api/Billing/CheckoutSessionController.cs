using System;
using System.Collections.Generic;
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
    [Route("billing/checkout-session")]
    [ApiController]
    public class CheckoutSessionController : ControllerBase {
        IMediator mediator;
        ISubscriptionConfig config;
        IDomainEventPublisher eventPublisher;

        public CheckoutSessionController(IMediator mediator, ISubscriptionConfig config, IDomainEventPublisher eventPublisher) {
            this.mediator = mediator;
            this.config = config;
            this.eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Create a new session with Stripe. User wishes to start checkout. Returns sessionId.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSession(CreateCheckoutSessionCommand create) {
            var billingRef = await mediator.Dispatch<CreateCheckoutSessionCommand, BillingReference>(
                create,
                User!.GetUserId()
            );

            return Ok(new {
                CheckoutSessionId = billingRef.BillingId
            });
        }

        /// <summary>
        /// Webhook to handle processing a session that completed successfully.
        /// </summary>
        [HttpPost("completed")]
        public async Task<IActionResult> CompleteSession() {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], config.WebhookSecret);

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted) {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session ?? throw new NullReferenceException();

                    await eventPublisher.Dispatch(new CheckoutSessionCompletedSuccessfully(
                        session.CustomerId
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