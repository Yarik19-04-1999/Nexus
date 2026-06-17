using Information.Api.Bot.Localization;
using Information.Application.Enums;

namespace Information.Api.Tests.Localization;

public class WeatherMessagesTests
{
    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void HourlyTitle_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(WeatherMessages.HourlyTitle(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void DailyTitle_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(WeatherMessages.DailyTitle(lang));

    [Theory]
    [MemberData(nameof(AllDayLanguageCombos))]
    public void AbbreviatedDayOfWeek_AllDaysAndLanguages_ReturnsNonEmpty(DayOfWeek day, BotLanguage lang)
        => Assert.NotEmpty(WeatherMessages.AbbreviatedDayOfWeek(day, lang));

    [Fact]
    public void AllMethods_WithInvalidLanguage_Throw()
    {
        var lang = (BotLanguage)int.MinValue;

        Assert.Throws<ArgumentOutOfRangeException>(() => WeatherMessages.HourlyTitle(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => WeatherMessages.DailyTitle(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => WeatherMessages.AbbreviatedDayOfWeek(DayOfWeek.Monday, lang));
    }

    public static IEnumerable<object[]> AllLanguages =>
        Enum.GetValues<BotLanguage>().Select(l => new object[] { l });

    public static IEnumerable<object[]> AllDayLanguageCombos =>
        from day in Enum.GetValues<DayOfWeek>()
        from lang in Enum.GetValues<BotLanguage>()
        select new object[] { day, lang };
}
