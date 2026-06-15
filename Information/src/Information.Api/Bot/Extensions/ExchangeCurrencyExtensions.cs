using Information.Application.Enums;

namespace Information.Api.Bot.Extensions;

public static class ExchangeCurrencyExtensions
{
    public static string ToFlag(this ExchangeCurrency currency) => currency switch
    {
        ExchangeCurrency.USD => "🇺🇸",
        ExchangeCurrency.EUR => "🇪🇺",
        ExchangeCurrency.GBP => "🇬🇧",
        _ => "💱"
    };
}
