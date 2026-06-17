using Nexus.Application.Core.Constants;

namespace Nexus.Application.Core.Exceptions;

public static class CommonExceptions
{
    public static DomainException ExternalProviderNoData() =>
        new(CommonErrorCodes.ExternalProviderNoData, CommonErrorMessages.ExternalProviderNoData(), canRetry: true);
}
