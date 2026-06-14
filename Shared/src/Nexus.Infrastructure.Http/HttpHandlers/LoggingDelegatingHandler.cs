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
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            await LogRequestBodyAsync(request, cancellationToken);
        }

        var sw = Stopwatch.StartNew();

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            var shouldLogBody = !response.IsSuccessStatusCode || _logger.IsEnabled(LogLevel.Debug);

            string? responseBody = null;
            if (shouldLogBody)
            {
                await response.Content.LoadIntoBufferAsync(cancellationToken);
                responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            }

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs}ms\n{Body}",
                    request.Method, request.RequestUri, (int)response.StatusCode, sw.ElapsedMilliseconds, responseBody);
            }
            else
            {
                _logger.LogWarning("HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs}ms\n{Body}",
                    request.Method, request.RequestUri, (int)response.StatusCode, sw.ElapsedMilliseconds, responseBody);
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

    private async Task LogRequestBodyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
        {
            return;
        }

        await request.Content.LoadIntoBufferAsync(cancellationToken);
        var body = await request.Content.ReadAsStringAsync(cancellationToken);

        if (!string.IsNullOrEmpty(body))
        {
            _logger.LogDebug("HTTP {Method} {Url} request body:\n{Body}",
                request.Method, request.RequestUri, body);
        }
    }
}
