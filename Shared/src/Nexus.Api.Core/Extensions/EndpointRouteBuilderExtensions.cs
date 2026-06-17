using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.Options;
using Scalar.AspNetCore;

namespace Nexus.Api.Core.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapNexus(this IEndpointRouteBuilder endpoints, NexusOptions options)
    {
        endpoints.MapNexusHealthChecks();

        if (options.UseOpenApi)
        {
            endpoints.MapNexusScalarUi();
        }

        return endpoints;
    }

    public static IEndpointRouteBuilder MapNexusScalarUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapOpenApi();
        endpoints.MapScalarApiReference();
        return endpoints;
    }

    public static IEndpointRouteBuilder MapNexusHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks(UrlConstants.Health.Live, new HealthCheckOptions
        {
            Predicate = _ => false,
        });

        endpoints.MapHealthChecks(UrlConstants.Health.Ready, new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(TagsConstants.HealthChecks.Ready),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        return endpoints;
    }
}
