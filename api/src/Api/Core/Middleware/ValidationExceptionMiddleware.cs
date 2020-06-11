using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;



namespace DetailingArsenal.Api {
    public class ValidationExceptionMiddlware : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
            if (context.Exception is ValidationException validationException) {
                var dto = new ValidationExceptionDto() {
                    Valid = validationException.Result.IsValid,
                    Errors = validationException.Result.Errors.Select(e => new ValidationFailureDto() {
                        Field = StringUtils.LowerCaseFirstLetter(e.Field)
                    ,
                        Message = e.Message
                    }).ToList()
                };

                context.Result = new ObjectResult(dto) {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}