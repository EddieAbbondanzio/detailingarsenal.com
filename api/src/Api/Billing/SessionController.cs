using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Billing;
using DetailingArsenal.Domain.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Billing {
    [Route("/billing/session")]
    [ApiController]
    public class SessionController : ControllerBase {
        private IMediator mediator;

        public SessionController(IMediator mediator) {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create a new session with Stripe. User wishes to start checkout. Returns sessionId.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSession() {
            var billingRef = await mediator.Dispatch<CreateSessionCommand, BillingReference>(
                new CreateSessionCommand(),
                User!.GetUserId()
            );

            return Ok(billingRef.BillingId);
        }

        /// <summary>
        /// Webhook to handle processing a session that completed successfully.
        /// </summary>
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteSession() {
            throw new NotImplementedException();
        }
    }
}