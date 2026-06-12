using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Nexus.Api.Core.Middleware;

internal static partial class ExceptionLoggingExtensions
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Domain exception: {ErrorCode} — {Message}")]
    internal static partial void LogDomainException(this ILogger logger, string errorCode, string message);

    [LoggerMessage(Level = LogLevel.Information, Message = "Request was cancelled")]
    internal static partial void LogRequestCancelled(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Bad HTTP request: {StatusCode} — {Message}")]
    internal static partial void LogBadHttpRequest(this ILogger logger, int statusCode, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "Unexpected exception")]
    internal static partial void LogUnexpectedException(this ILogger logger, Exception exception);
}
