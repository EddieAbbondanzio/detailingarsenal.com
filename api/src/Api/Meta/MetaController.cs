using System;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api {
    [ApiController]
    [Route("meta")]
    public class MetaController : ControllerBase {
        private IUserGateway userService;

        public MetaController(IUserGateway userService) {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok("420 blaze it");
        }
    }
}