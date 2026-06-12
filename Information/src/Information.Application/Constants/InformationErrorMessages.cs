using Information.Application.Enums;

namespace Information.Application.Constants;

public static class InformationErrorMessages
{
    public static string ProviderUnavailable(string provider) =>
        $"Provider '{provider}' is unavailable. Please try again later.";

    public static string RateNotFound(ExchangeCurrency currency, DateOnly date) =>
        $"Exchange rate for '{currency}' on {date:yyyy-MM-dd} was not found.";
}
