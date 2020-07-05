using System;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Users;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DetailingArsenal.Api {
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase {
        private IMediator mediator;

        public UserController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser() {
            var user = await mediator.Dispatch<GetUserByAuth0IdQuery, UserView>(new GetUserByAuth0IdQuery(), User.GetUserId());
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserCommand command) {
            var user = await mediator.Dispatch<UpdateUserCommand, UserView>(command, User.GetUserId());
            return Ok(user);
        }
    }
}