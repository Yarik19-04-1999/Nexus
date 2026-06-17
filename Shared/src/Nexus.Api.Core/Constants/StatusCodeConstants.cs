using Microsoft.AspNetCore.Http;
using System.Net;

namespace Nexus.Api.Core.Constants;

public class StatusCodeConstants
{
    public const int DomainError = StatusCodes.Status418ImATeapot;
    public const HttpStatusCode DomainErrorStatusCode = (HttpStatusCode)DomainError;

    public const int InternalError = StatusCodes.Status500InternalServerError;
    public const HttpStatusCode InternalErrorStatusCode = (HttpStatusCode)InternalError;
}
