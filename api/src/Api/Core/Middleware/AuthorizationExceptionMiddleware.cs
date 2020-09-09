using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DetailingArsenal.Api {
    public class AuthorizationExceptionMiddlware : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
            if (context.Exception is AuthorizationException authorizationException) {
                var dto = new AuthorizationExceptionDto() {
                    Message = authorizationException.Message
                };

                context.Result = new ObjectResult(dto) {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
        }
    }
}