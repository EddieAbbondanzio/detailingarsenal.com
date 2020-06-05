using System;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api {
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase {
        public CustomerController() {
        }
    }
}