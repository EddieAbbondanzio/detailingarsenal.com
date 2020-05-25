using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware {
    private readonly RequestDelegate next;
    private ILogger logger;

    public ExceptionHandlingMiddleware(ILogger logger, RequestDelegate next) {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context) {
        try {
            throw new Exception("test lol");
            // await next(context);
        } catch (Exception ex) {
            logger.LogError(new EventId(1), ex, "Something went really wrong");
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync("System borked. Please stand by.");
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions {
    public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder builder) {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}