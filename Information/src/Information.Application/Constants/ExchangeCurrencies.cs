using Information.Application.Enums;

namespace Information.Application.Constants;

public static class ExchangeCurrencies
{
    public static readonly ExchangeCurrency[] All = Enum.GetValues<ExchangeCurrency>();
}
