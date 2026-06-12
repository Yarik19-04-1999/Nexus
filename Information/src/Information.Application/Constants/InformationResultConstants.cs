using Information.Application.Enums;
using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Constants;

public static class InformationResultConstants
{
    public static Result<T> ProviderUnavailable<T>(string provider, bool canRetry = true) =>
        Result<T>.Failure(InformationErrorCodes.ProviderUnavailable, InformationErrorMessages.ProviderUnavailable(provider), canRetry);

    public static Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>> RatesNotFound(ExchangeCurrency currency, DateOnly date) =>
        Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>.Failure(InformationErrorCodes.RateNotFound, InformationErrorMessages.RateNotFound(currency, date));

    public static Result<IReadOnlyList<ExchangeRateHistory>> RateHistoryNotFound(ExchangeCurrency currency, DateOnly date) =>
        Result<IReadOnlyList<ExchangeRateHistory>>.Failure(InformationErrorCodes.RateNotFound, InformationErrorMessages.RateNotFound(currency, date));
}
