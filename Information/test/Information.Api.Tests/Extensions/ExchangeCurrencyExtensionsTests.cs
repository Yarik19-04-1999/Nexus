using Information.Api.Bot.Extensions;
using Information.Application.Enums;

namespace Information.Api.Tests.Extensions;

public class ExchangeCurrencyExtensionsTests
{
    [Theory]
    [MemberData(nameof(AllCurrencies))]
    public void ToFlag_AllCurrencies_ReturnsNonEmpty(ExchangeCurrency currency)
        => Assert.NotEmpty(currency.ToFlag());

    [Fact]
    public void ToFlag_WithInvalidCurrency_Throws()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ((ExchangeCurrency)int.MinValue).ToFlag());

        Assert.Equal("currency", ex.ParamName);
    }

    public static IEnumerable<object[]> AllCurrencies =>
        Enum.GetValues<ExchangeCurrency>().Select(c => new object[] { c });
}
