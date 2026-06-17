using Nexus.Application.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Nexus.Application.Core.Models;

public class Result : IResult
{
    private static readonly Result CachedRetryableSuccess = new(canRetry: true);
    private static readonly Result CachedNonRetryableSuccess = new();

    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public bool CanRetry { get; }

    [MemberNotNullWhen(true, nameof(ErrorCode))]
    public virtual bool HasError { get; }

    [MemberNotNullWhen(false, nameof(ErrorCode))]
    public virtual bool IsSuccess => !HasError;

    protected Result(bool canRetry = false)
    {
        HasError = false;
        CanRetry = canRetry;
    }

    protected Result(string errorCode, string? errorMessage = null, bool canRetry = false)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        HasError = true;
        CanRetry = canRetry;
    }

    public static Result Success(bool canRetry = false)
        => canRetry ? CachedRetryableSuccess : CachedNonRetryableSuccess;

    public static Result Failure(string errorCode, string? errorMessage = null, bool canRetry = false)
        => new(errorCode, errorMessage, canRetry);
}

public class Result<T> : Result
{
    [MemberNotNullWhen(true, nameof(Data))]
    public override bool IsSuccess => base.IsSuccess;

    [MemberNotNullWhen(false, nameof(Data))]
    public override bool HasError => base.HasError;

    public T? Data { get; }

    private Result(T data, bool canRetry = false)
        : base(canRetry)
    {
        Data = data ?? throw new InvalidOperationException($"{nameof(Data)} should be not null on success result");
    }

    private Result(string errorCode, string? errorMessage = null, bool canRetry = false)
        : base(errorCode, errorMessage, canRetry)
    {
    }

    public static Result<T> Success(T data, bool canRetry = false) => new(data, canRetry);

    public new static Result<T> Failure(string errorCode, string? errorMessage = null, bool canRetry = false)
        => new(errorCode, errorMessage, canRetry);
}

public class NullableResult<T> : Result
{
    public override bool IsSuccess => base.IsSuccess;

    [MemberNotNullWhen(false, nameof(Data))]
    public override bool HasError => base.HasError;

    public T? Data { get; }

    private NullableResult(T? data = default, bool canRetry = false)
        : base(canRetry)
    {
        Data = data;
    }

    private NullableResult(string errorCode, string? errorMessage = null, bool canRetry = false)
        : base(errorCode, errorMessage, canRetry)
    {
    }

    public static NullableResult<T> Success(T? data = default, bool canRetry = false) => new(data, canRetry);

    public new static NullableResult<T> Failure(string errorCode, string? errorMessage = null, bool canRetry = false)
        => new(errorCode, errorMessage, canRetry);
}
