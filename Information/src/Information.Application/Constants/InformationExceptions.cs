using Nexus.Application.Core.Exceptions;

namespace Information.Application.Constants;

public static class InformationExceptions
{
    public static DomainException ProviderUnavailable(string provider) =>
        new(InformationErrorCodes.ProviderUnavailable, InformationErrorMessages.ProviderUnavailable(provider), canRetry: true);
}
