using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.CorrelationId;
using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Exceptions;

namespace Nexus.Api.Core.Middleware;

public class ExceptionResponseMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        if (ex is DomainException domainException)
        {
            context.Response.StatusCode = StatusCodes.Status418ImATeapot;
            await context.Response.WriteAsJsonAsync(
                new DomainErrorResponse(domainException.ErrorCode, domainException.Message, domainException.CanRetry));
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var correlationId = context.RequestServices.GetService<ICorrelationIdAccessor>()?.CorrelationId
                ?? context.TraceIdentifier;
            await context.Response.WriteAsJsonAsync(new UnexpectedErrorResponse(correlationId));
        }
    }
}
