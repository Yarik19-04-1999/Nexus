using CorrelationId.Abstractions;

namespace Nexus.Infrastructure.Http.HttpHandlers;

public class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    public CorrelationIdDelegatingHandler(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContextAccessor = correlationContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId = _correlationContextAccessor.CorrelationContext?.CorrelationId;

        if (!string.IsNullOrEmpty(correlationId))
        {
            request.Headers.TryAddWithoutValidation(CorrelationIdHeader, correlationId);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
