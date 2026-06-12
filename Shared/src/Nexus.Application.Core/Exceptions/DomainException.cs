namespace Nexus.Application.Core.Exceptions;

public class DomainException : Exception
{
    public string ErrorCode { get; }
    public bool CanRetry { get; }

    public DomainException(string errorCode, string message, bool canRetry = false)
        : base(message)
    {
        ErrorCode = errorCode;
        CanRetry = canRetry;
    }
}
