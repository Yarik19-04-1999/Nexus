using CorrelationId.Abstractions;

namespace Nexus.Api.Core.Extensions;

public static class CorrelationContextAccessorExtensions
{
    public static string GetCorrelationId(this ICorrelationContextAccessor correlationContextAccessor)
    {
        return correlationContextAccessor.CorrelationContext.CorrelationId;
    }
}
