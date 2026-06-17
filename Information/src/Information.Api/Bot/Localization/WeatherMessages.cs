using Information.Application.Enums;
using Information.Application.Extensions;

namespace Information.Api.Bot.Localization;

public static class WeatherMessages
{
    public static string HourlyTitle(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "почасовой прогноз",
        BotLanguage.Ukrainian => "погодинний прогноз",
        BotLanguage.English => "hourly forecast",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string DailyTitle(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "прогноз на 5 дней",
        BotLanguage.Ukrainian => "прогноз на 5 днів",
        BotLanguage.English => "5-day forecast",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string AbbreviatedDayOfWeek(DayOfWeek day, BotLanguage lang)
    {
        var name = lang.ToCulture().DateTimeFormat.GetAbbreviatedDayName(day);
        return char.ToUpper(name[0]) + name[1..];
    }
}
