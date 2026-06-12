using Microsoft.AspNetCore.Mvc;

namespace Nexus.Api.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NexusRouteAttribute : RouteAttribute
{
    public NexusRouteAttribute() : base("api/v{version:apiVersion}/[controller]")
    {
    }
}
