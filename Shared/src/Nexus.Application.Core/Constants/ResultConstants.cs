using Nexus.Application.Core.Models;

namespace Nexus.Application.Core.Constants;

public static class ResultConstants
{
    public static Result<T> NotFound<T>(int id, bool canRetry = false)
        => Result<T>.Failure(CommonErrorCodes.NotFound, CommonErrorMessages.NotFound<T>(id), canRetry);

    public static Result<T> NotFound<T>(string code, bool canRetry = false)
        => Result<T>.Failure(CommonErrorCodes.NotFound, CommonErrorMessages.NotFound<T>(code), canRetry);

    public static Result AlreadyExpired<T>(int id, bool canRetry = false)
        => Result.Failure(CommonErrorCodes.AlreadyExpired, CommonErrorMessages.AlreadyExpired<T>(id), canRetry);

    public static Result CodeAlreadyExists<T>(string code, bool canRetry = false)
        => Result.Failure(CommonErrorCodes.CodeAlreadyExists, CommonErrorMessages.CodeAlreadyExists<T>(code), canRetry);
}
