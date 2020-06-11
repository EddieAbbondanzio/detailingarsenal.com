using System.Net;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DetailingArsenal.Api {
    public class SpecificationExceptionMiddleware : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
            if (context.Exception is SpecificationException specificationException) {
                var dto = new SpecifcationExceptionDto() {
                    IsSatisfied = specificationException.Result.IsSatisfied,
                    Message = specificationException.Result.Messages[0] // First error should be more than enough?
                };

                context.Result = new ObjectResult(dto) {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}