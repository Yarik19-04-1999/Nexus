namespace Nexus.Api.Core.ViewModels;

public record DomainErrorResponse(
    string ErrorCode, 
    string? ErrorMessage = null, 
    bool CanRetry = false);
