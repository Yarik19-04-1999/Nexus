using Microsoft.AspNetCore.Builder;
using Nexus.Api.Core.CorrelationId;
using Nexus.Api.Core.Middleware;

namespace Nexus.Api.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseNexusCorrelationId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }

    public static IApplicationBuilder UseNexusExceptionHandling(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<ExceptionResponseMiddleware>()
            .UseMiddleware<ExceptionLoggingMiddleware>();
    }
}
