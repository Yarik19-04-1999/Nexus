using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nexus.Application.Core.Exceptions;
using System.Runtime.ExceptionServices;

namespace Nexus.Api.Core.Middleware;

public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var dispatchInfo = ExceptionDispatchInfo.Capture(ex);
            LogException(dispatchInfo.SourceException);
            dispatchInfo.Throw();
        }
    }

    private void LogException(Exception ex)
    {
        switch (ex)
        {
            case DomainException domainException:
                logger.LogDomainException(domainException.ErrorCode, domainException.Message);
                break;
            case OperationCanceledException:
                logger.LogRequestCancelled();
                break;
            case BadHttpRequestException badRequest:
                logger.LogBadHttpRequest(badRequest.StatusCode, badRequest.Message);
                break;
            default:
                logger.LogUnexpectedException(ex);
                break;
        }
    }
}
