using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Nexus.Api.Core.Middleware;

namespace Nexus.Api.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseNexusCorrelationId(this IApplicationBuilder app)
        => app.UseCorrelationId();

    public static IApplicationBuilder UseNexusExceptionHandling(this IApplicationBuilder app)
        => app
            .UseMiddleware<ExceptionResponseMiddleware>()
            .UseMiddleware<ExceptionLoggingMiddleware>();
}
