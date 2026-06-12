namespace Nexus.Api.Core.CorrelationId;

public interface ICorrelationIdAccessor
{
    string CorrelationId { get; set; }
}
