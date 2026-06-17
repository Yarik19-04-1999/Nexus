using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Nexus.Api.Core.Middleware;

public static partial class ExceptionLoggingExtensions
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Domain exception: {ErrorCode} — {Message}")]
    public static partial void LogDomainException(this ILogger logger, string errorCode, string message);

    [LoggerMessage(Level = LogLevel.Information, Message = "Request was cancelled")]
    public static partial void LogRequestCancelled(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Bad HTTP request: {StatusCode} — {Message}")]
    public static partial void LogBadHttpRequest(this ILogger logger, int statusCode, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "Unexpected exception")]
    public static partial void LogUnexpectedException(this ILogger logger, Exception exception);
}
