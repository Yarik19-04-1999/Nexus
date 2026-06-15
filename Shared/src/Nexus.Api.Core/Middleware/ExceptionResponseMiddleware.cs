using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.Mappers;
using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Exceptions;
using System.Net.Mime;

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
        context.Response.ContentType = MediaTypeNames.Application.Json;

        if (ex is DomainException domainException)
        {
            context.Response.StatusCode = StatusCodes.Status418ImATeapot;
            await context.Response.WriteAsJsonAsync(ResponsesMappers.Map(domainException));
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var correlationId = context.RequestServices.GetService<ICorrelationContextAccessor>()?.CorrelationContext?.CorrelationId
                ?? context.TraceIdentifier;
            await context.Response.WriteAsJsonAsync(new UnexpectedErrorResponse(correlationId));
        }
    }
}
