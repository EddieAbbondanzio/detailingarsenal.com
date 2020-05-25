using Auth0.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware {
    private readonly RequestDelegate next;

    public ExceptionHandlingMiddleware(RequestDelegate next) {
        this.next = next;
    }

    public async Task Invoke(HttpContext context) {
        try {
            await next(context);
        } catch (ErrorApiException ex) {
            Log.Fatal($"Auth0 failed with code: {ex.StatusCode}, message: {ex.ApiError.Message}", ex);
            throw;
        } catch (Exception ex) {
            Log.Fatal(ex, "Something went really wrong!");
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync("System borked. Please stand by.");
            throw;
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions {
    public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder builder) {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}