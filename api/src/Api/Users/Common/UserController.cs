using System;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Application.Users;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DetailingArsenal.Api.Users {
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
            var user = await mediator.Dispatch<GetUserByAuth0IdQuery, UserReadModel>(User);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateCommand command) {
            await mediator.Dispatch<UserUpdateCommand, CommandResult>(command, User);
            return Ok();
        }
    }
}