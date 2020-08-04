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
        IBillingWebhookParser webhookParser;
        IDomainEventPublisher eventPublisher;

        public CustomerController(IMediator mediator, IBillingWebhookParser eventParser, IDomainEventPublisher eventPublisher) {
            this.mediator = mediator;
            this.webhookParser = eventParser;
            this.eventPublisher = eventPublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer() {
            var sub = await mediator.Dispatch<GetCustomerQuery, CustomerReadModel>(
                new GetCustomerQuery(),
                User.GetUserId()
            );

            return Ok(sub);
        }

        [Authorize]
        [HttpPatch("subscription")]
        public async Task<IActionResult> UndoCancellingSubscription() {
            await mediator.Dispatch(new UndoCancellingSubscriptionCommand(), User.GetUserId());
            return Ok();
        }

        [Authorize]
        [HttpDelete("subscription")]
        public async Task<IActionResult> CancelSubscriptionAtPeriodEnd() {
            await mediator.Dispatch(new CancelSubscriptionAtPeriodEndCommand(), User.GetUserId());
            return Ok();
        }

        [HttpPost("subscription/trial-will-end")]
        public async Task<IActionResult> TrialWillEnd() {
            var trialEndingEvent = await webhookParser.Parse<CustomerTrialWillEndSoon>(
                Request.Body,
                Request.Headers[Headers.StripeSignature]
            );

            if (trialEndingEvent != null) {
                _ = eventPublisher.Dispatch(trialEndingEvent);
            }

            return Ok(420);
        }

        [HttpPost("subscription/invoice/updated")]
        public async Task<IActionResult> InvoicePaymentSucceeded() {
            var invoiceUpdatedEvent = await webhookParser.Parse<CustomerSubscriptionInvoiceUpdated>(
                Request.Body,
                Request.Headers[Headers.StripeSignature]
            );


            if (invoiceUpdatedEvent != null) {
                _ = eventPublisher.Dispatch(invoiceUpdatedEvent);
            }

            return Ok("ayy lmao");
        }
    }
}