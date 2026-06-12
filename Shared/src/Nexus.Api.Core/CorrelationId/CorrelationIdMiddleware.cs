using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Nexus.Api.Core.CorrelationId;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public const string HeaderName = "X-Correlation-Id";

    public async Task Invoke(HttpContext context, ICorrelationIdAccessor correlationIdAccessor)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        correlationIdAccessor.CorrelationId = correlationId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        using (logger.BeginScope(new Dictionary<string, object> { [HeaderName] = correlationId }))
        {
            await next(context);
        }
    }
}
