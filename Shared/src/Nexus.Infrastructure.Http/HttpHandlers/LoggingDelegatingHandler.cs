using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Nexus.Infrastructure.Http.HttpHandlers;

public class LoggingDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingDelegatingHandler> _logger;

    public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs}ms",
                    request.Method, request.RequestUri, (int)response.StatusCode, sw.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogWarning("HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs}ms",
                    request.Method, request.RequestUri, (int)response.StatusCode, sw.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "HTTP {Method} {Url} failed after {ElapsedMs}ms",
                request.Method, request.RequestUri, sw.ElapsedMilliseconds);
            throw;
        }
    }
}
