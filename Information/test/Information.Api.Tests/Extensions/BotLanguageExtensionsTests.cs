using Information.Application.Enums;
using Information.Application.Extensions;

namespace Information.Api.Tests.Extensions;

public class BotLanguageExtensionsTests
{
    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void ToCulture_AllLanguages_ReturnsNonNull(BotLanguage lang)
        => Assert.NotNull(lang.ToCulture());

    [Fact]
    public void ToCulture_WithInvalidLanguage_Throws()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ((BotLanguage)int.MinValue).ToCulture());

        Assert.Equal("lang", ex.ParamName);
    }

    public static IEnumerable<object[]> AllLanguages =>
        Enum.GetValues<BotLanguage>().Select(l => new object[] { l });
}
