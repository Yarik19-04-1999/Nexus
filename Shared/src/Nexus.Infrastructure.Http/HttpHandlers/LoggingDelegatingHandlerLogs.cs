using System.Net;
using Microsoft.Extensions.Logging;

namespace Nexus.Infrastructure.Http.HttpHandlers;

internal static partial class LoggingDelegatingHandlerLogs
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "HTTP {Method} {Url} request body:\n{Body}")]
    internal static partial void RequestBody(this ILogger logger, HttpMethod method, Uri? url, string body);

    [LoggerMessage(Level = LogLevel.Debug, Message = "HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs}ms\n{Body}")]
    internal static partial void SuccessResponse(this ILogger logger, HttpMethod method, Uri? url, HttpStatusCode statusCode, long elapsedMs, string? body);

    [LoggerMessage(Level = LogLevel.Warning, Message = "HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs}ms\n{Body}")]
    internal static partial void ErrorResponse(this ILogger logger, HttpMethod method, Uri? url, HttpStatusCode statusCode, long elapsedMs, string? body);

    [LoggerMessage(Level = LogLevel.Error, Message = "HTTP {Method} {Url} failed after {ElapsedMs}ms")]
    internal static partial void RequestException(this ILogger logger, Exception exception, HttpMethod method, Uri? url, long elapsedMs);
}
