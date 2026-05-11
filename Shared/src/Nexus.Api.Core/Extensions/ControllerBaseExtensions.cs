using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Mappers;
using Nexus.Application.Core.Models;

namespace Nexus.Api.Core.Extensions;

public static class ControllerBaseExtensions
{
    public static ObjectResult DomainError(this ControllerBase controller, Result result)
        => new(result.MapToDomainErrorResponse()) 
        { 
            StatusCode = StatusCodes.Status418ImATeapot 
        };
}
