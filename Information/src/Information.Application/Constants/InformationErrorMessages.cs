using Information.Application.Enums;
using Nexus.Application.Core.Constants;

namespace Information.Application.Constants;

public static class InformationErrorMessages
{
    public static string RateNotFound(ExchangeCurrency currency, DateOnly date) =>
        $"Exchange rate for '{currency}' on {date.ToString(CommonConstants.DateShortFormat)} was not found.";
}
