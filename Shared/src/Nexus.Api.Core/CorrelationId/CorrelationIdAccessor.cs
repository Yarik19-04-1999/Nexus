namespace Nexus.Api.Core.CorrelationId;

internal class CorrelationIdAccessor : ICorrelationIdAccessor
{
    public string CorrelationId { get; set; } = string.Empty;
}
