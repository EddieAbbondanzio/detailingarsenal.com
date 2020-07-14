using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Billing;
using DetailingArsenal.Domain.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Billing {
    [Authorize]
    [ApiController]
    [Route("/billing/customer")]
    public class CustomerController : ControllerBase {
        IMediator mediator;

        public CustomerController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer() {
            var sub = await mediator.Dispatch<GetCustomerQuery, CustomerReadModel>(
                new GetCustomerQuery(),
                User.GetUserId()
            );

            return Ok(sub);
        }
    }
}