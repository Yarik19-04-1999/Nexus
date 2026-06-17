using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Nexus.Api.Core.Middleware;
using Nexus.Api.Core.Options;

namespace Nexus.Api.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseNexus(this IApplicationBuilder app, NexusOptions options)
    {
        app.UseHttpsRedirection()
           .UseNexusSecurityHeaders()
           .UseNexusCorrelationId()
           .UseNexusExceptionHandling();

        if (options.UseResponseCompression)
        {
            app.UseNexusResponseCompression();
        }

        if (options.HasRequestTimeout())
        {
            app.UseNexusRequestTimeouts();
        }

        return app;
    }

    public static IApplicationBuilder UseNexusCorrelationId(this IApplicationBuilder app)
        => app.UseCorrelationId();

    public static IApplicationBuilder UseNexusExceptionHandling(this IApplicationBuilder app)
        => app
            .UseMiddleware<ExceptionResponseMiddleware>()
            .UseMiddleware<ExceptionLoggingMiddleware>();

    public static IApplicationBuilder UseNexusSecurityHeaders(this IApplicationBuilder app)
        => app.UseSecurityHeaders(policy => policy
            .AddFrameOptionsDeny()
            .AddXssProtectionDisabled()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader());

    public static IApplicationBuilder UseNexusResponseCompression(this IApplicationBuilder app)
        => app.UseResponseCompression();

    public static IApplicationBuilder UseNexusRequestTimeouts(this IApplicationBuilder app)
        => app.UseRequestTimeouts();
}
