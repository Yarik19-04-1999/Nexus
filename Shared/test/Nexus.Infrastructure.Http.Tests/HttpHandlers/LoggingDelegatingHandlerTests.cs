using Microsoft.Extensions.Logging;
using Nexus.Infrastructure.Http.HttpHandlers;
using System.Net;

namespace Nexus.Infrastructure.Http.Tests.HttpHandlers;

public class LoggingDelegatingHandlerTests
{
    private class TrackingLogger : ILogger<LoggingDelegatingHandler>
    {
        public List<(LogLevel Level, string Message, Exception? Exception)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, formatter(state, exception), exception));
        }
    }

    private class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _respond;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> respond)
        {
            _respond = respond;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => Task.FromResult(_respond(request));
    }

    private class ThrowingHandler : HttpMessageHandler
    {
        private readonly Exception _exception;

        public ThrowingHandler(Exception exception)
        {
            _exception = exception;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => throw _exception;
    }

    private static (LoggingDelegatingHandler handler, TrackingLogger logger) CreateHandler(
        HttpMessageHandler innerHandler)
    {
        var logger = new TrackingLogger();
        var handler = new LoggingDelegatingHandler(logger)
        {
            InnerHandler = innerHandler
        };
        return (handler, logger);
    }

    [Fact]
    public async Task SendAsync_WhenResponseIsSuccess_LogsAtDebugLevel()
    {
        var (handler, logger) = CreateHandler(new StubHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)));

        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");

        await invoker.SendAsync(request, CancellationToken.None);

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Debug);
    }

    [Fact]
    public async Task SendAsync_WhenResponseIsNotSuccess_LogsAtWarningLevel()
    {
        var (handler, logger) = CreateHandler(new StubHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError)));

        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");

        await invoker.SendAsync(request, CancellationToken.None);

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Warning);
    }

    [Fact]
    public async Task SendAsync_WhenInnerHandlerThrows_LogsAtErrorLevelAndRethrows()
    {
        var expectedException = new InvalidOperationException("network error");
        var (handler, logger) = CreateHandler(new ThrowingHandler(expectedException));

        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/test");

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(
            () => invoker.SendAsync(request, CancellationToken.None));

        Assert.Same(expectedException, thrown);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
