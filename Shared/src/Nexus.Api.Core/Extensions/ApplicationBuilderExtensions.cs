using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nexus.Api.Core.Middleware;
using Scalar.AspNetCore;

namespace Nexus.Api.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseNexusCorrelationId(this IApplicationBuilder app)
        => app.UseCorrelationId();

    public static IApplicationBuilder UseNexusExceptionHandling(this IApplicationBuilder app)
        => app
            .UseMiddleware<ExceptionResponseMiddleware>()
            .UseMiddleware<ExceptionLoggingMiddleware>();


    public static IEndpointRouteBuilder MapNexusScalarUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapOpenApi();
        endpoints.MapScalarApiReference();
        return endpoints;
    }
}
