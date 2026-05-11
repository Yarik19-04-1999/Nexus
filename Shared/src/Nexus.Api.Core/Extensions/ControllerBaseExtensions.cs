using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Mappers;
using Nexus.Application.Core.Models;

namespace Nexus.Api.Core.Extensions;

public static class ControllerBaseExtensions
{
    public static ObjectResult DomainError(this ControllerBase controller, Result result)
        => controller.StatusCode(StatusCodes.Status418ImATeapot , result.MapToDomainErrorResponse());
}
