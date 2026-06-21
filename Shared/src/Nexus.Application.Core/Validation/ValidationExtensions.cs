using FluentValidation.Results;
using Nexus.Application.Core.Models;

namespace Nexus.Application.Core.Validation;

public static class ValidationExtensions
{
    public static Result ToResult(this ValidationResult validationResult)
    {
        var failure = validationResult.Errors.FirstOrDefault(e => e.ErrorCode != null);
        if (failure == null)
        {
            return Result.Success();
        }

        return Result.Failure(failure.ErrorCode, failure.ErrorMessage);
    }

    public static Result<T> ToResult<T>(this ValidationResult validationResult)
    {
        var failure = validationResult.Errors.FirstOrDefault(e => e.ErrorCode != null);
        if (failure == null)
        {
            throw new InvalidOperationException("Cannot convert a valid ValidationResult to a failure Result<T>.");
        }

        return Result<T>.Failure(failure.ErrorCode, failure.ErrorMessage);
    }

    public static Result ToResult(this ValidationFailure failure)
        => Result.Failure(failure.ErrorCode, failure.ErrorMessage);
}
