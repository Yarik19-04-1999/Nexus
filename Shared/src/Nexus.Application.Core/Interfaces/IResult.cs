namespace Nexus.Application.Core.Interfaces;

public interface IResult
{
    public bool HasError { get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public bool CanRetry { get; }
}
