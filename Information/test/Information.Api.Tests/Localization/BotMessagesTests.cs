using Information.Api.Bot.Localization;
using Information.Application.Enums;

namespace Information.Api.Tests.Localization;

public class BotMessagesTests
{
    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void Welcome_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.Welcome(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void ChooseLanguage_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.ChooseLanguage(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void MainMenu_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.MainMenu(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnWeather_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnWeather(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnRatesToday_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnRatesToday(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnRatesHistory_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnRatesHistory(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnEpicGames_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnEpicGames(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnLanguage_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnLanguage(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnBack_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnBack(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnBackToMenu_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnBackToMenu(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void ChooseCity_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.ChooseCity(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void ChooseWeatherType_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.ChooseWeatherType(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnHourly_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnHourly(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnDaily_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnDaily(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void ChooseCurrencyHistory_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.ChooseCurrencyHistory(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void BtnAllCurrencies_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.BtnAllCurrencies(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void NoEpicGames_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.NoEpicGames(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void EpicGamesHeader_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.EpicGamesHeader(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void FreeUntilLabel_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.FreeUntilLabel(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void GetLabel_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.GetLabel(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void RatesTodayHeader_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.RatesTodayHeader(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void RatesHistoryHeader_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.RatesHistoryHeader(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void YesterdayLabel_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.YesterdayLabel(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void WeekAgoLabel_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.WeekAgoLabel(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void MonthAgoLabel_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.MonthAgoLabel(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void YearAgoLabel_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.YearAgoLabel(lang));

    [Theory]
    [MemberData(nameof(AllLanguages))]
    public void UnexpectedError_AllLanguages_ReturnsNonEmpty(BotLanguage lang)
        => Assert.NotEmpty(BotMessages.UnexpectedError(lang));

    [Theory]
    [MemberData(nameof(AllCityLanguageCombos))]
    public void GetCityName_AllCitiesAndLanguages_ReturnsNonEmpty(WeatherCity city, BotLanguage lang)
        => Assert.NotEmpty(BotMessages.GetCityName(city, lang));

    [Fact]
    public void AllMethods_WithInvalidLanguage_Throw()
    {
        var lang = (BotLanguage)int.MinValue;

        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.Welcome(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.ChooseLanguage(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.MainMenu(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnWeather(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnRatesToday(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnRatesHistory(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnEpicGames(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnLanguage(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnBack(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnBackToMenu(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.ChooseCity(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.ChooseWeatherType(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnHourly(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnDaily(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.ChooseCurrencyHistory(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.BtnAllCurrencies(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.NoEpicGames(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.EpicGamesHeader(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.FreeUntilLabel(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.GetLabel(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.RatesTodayHeader(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.RatesHistoryHeader(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.YesterdayLabel(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.WeekAgoLabel(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.MonthAgoLabel(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.YearAgoLabel(lang));
        Assert.Throws<ArgumentOutOfRangeException>(() => BotMessages.UnexpectedError(lang));
    }

    [Fact]
    public void GetCityName_WithInvalidLanguage_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BotMessages.GetCityName(WeatherCity.Kyiv, (BotLanguage)int.MinValue));
    }

    [Fact]
    public void GetCityName_WithInvalidCity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BotMessages.GetCityName((WeatherCity)int.MinValue, BotLanguage.Ukrainian));
    }

    public static IEnumerable<object[]> AllLanguages =>
        Enum.GetValues<BotLanguage>().Select(l => new object[] { l });

    public static IEnumerable<object[]> AllCityLanguageCombos =>
        from city in Enum.GetValues<WeatherCity>()
        from lang in Enum.GetValues<BotLanguage>()
        select new object[] { city, lang };
}
