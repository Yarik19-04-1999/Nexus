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
            await LogRequestAsync(request, cancellationToken);
        }

        var sw = Stopwatch.StartNew();

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            await LogResponseAsync(request, response, sw.ElapsedMilliseconds, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.RequestException(ex, request.Method, request.RequestUri, sw.ElapsedMilliseconds);
            throw;
        }
    }

    private async Task LogRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
        {
            return;
        }

        await request.Content.LoadIntoBufferAsync(cancellationToken);
        var body = await request.Content.ReadAsStringAsync(cancellationToken);

        if (!string.IsNullOrEmpty(body))
        {
            _logger.RequestBody(request.Method, request.RequestUri, body);
        }
    }

    private async Task LogResponseAsync(HttpRequestMessage request, HttpResponseMessage response, long elapsedMs, CancellationToken cancellationToken)
    {
        var shouldReadBody = !response.IsSuccessStatusCode || _logger.IsEnabled(LogLevel.Debug);

        string? body = null;
        if (shouldReadBody)
        {
            await response.Content.LoadIntoBufferAsync(cancellationToken);
            body = await response.Content.ReadAsStringAsync(cancellationToken);
        }

        if (response.IsSuccessStatusCode)
        {
            _logger.SuccessResponse(request.Method, request.RequestUri, response.StatusCode, elapsedMs, body);
        }
        else
        {
            _logger.ErrorResponse(request.Method, request.RequestUri, response.StatusCode, elapsedMs, body);
        }
    }
}
