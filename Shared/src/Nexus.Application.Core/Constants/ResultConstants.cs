using Nexus.Application.Core.Models;

namespace Nexus.Application.Core.Constants;

public static class ResultConstants
{
    public static Result<T> NotFound<T>(int id, bool canRetry = false)
    {
        return Result<T>.Failure(CommonErrorCodes.NotFound, CommonErrorMessages.NotFound<T>(id), canRetry);
    }

    public static Result AlreadyExpired(bool canRetry = false)
    {
        return Result.Failure(CommonErrorCodes.AlreadyExpired, CommonErrorMessages.AlreadyExpired, canRetry);
    }
}
