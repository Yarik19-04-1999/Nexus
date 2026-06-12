using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Nexus.Api.Core.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapNexusScalarUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapOpenApi();
        endpoints.MapScalarApiReference();
        return endpoints;
    }
}
