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
        IBillingWebhookParser webhookParser;
        IDomainEventPublisher eventPublisher;

        public CheckoutSessionController(IMediator mediator, IBillingWebhookParser eventParser, IDomainEventPublisher eventPublisher) {
            this.mediator = mediator;
            this.webhookParser = eventParser;
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
            var successEvent = await webhookParser.Parse<CheckoutSessionCompletedSuccessfully>(
                Request.Body,
                Request.Headers[Headers.StripeSignature]
            );

            if (successEvent != null) {
                _ = eventPublisher.Dispatch(successEvent);
            }

            return Ok(420);
        }
    }
}