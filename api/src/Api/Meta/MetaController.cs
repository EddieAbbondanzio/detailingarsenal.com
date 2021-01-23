using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DetailingArsenal.Api.Meta {
    [ApiController]
    [Route("meta")]
    public class MetaController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            return Ok("420 blaze it");
        }

        [HttpGet("test")]
        public IActionResult Test() {
            return Ok("Yeah test works mate");
        }
    }
}