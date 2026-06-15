using System.Text;
using Information.Application.Enums;
using Information.Application.Models;
using Information.Api.Bot.Localization;

namespace Information.Api.Bot.Formatters;

internal static class WeatherFormatter
{
    internal static string FormatHourly(IReadOnlyList<HourlyWeather> items, WeatherCity city, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"🌤 {BotMessages.GetCityName(city, lang)} — {FormatHourlyTitle(lang)}");
        sb.AppendLine();

        foreach (var item in items)
        {
            var temp = (int)Math.Round(item.Temperature);
            var emoji = GetWeatherEmoji(item.WeatherCode);
            sb.AppendLine($"{item.Time:HH:mm}  {emoji} {temp:+#;-#;0}°  💧{item.PrecipitationProbability}%");
        }

        return sb.ToString().TrimEnd();
    }

    internal static string FormatDaily(IReadOnlyList<DailyWeather> items, WeatherCity city, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"📅 {BotMessages.GetCityName(city, lang)} — {FormatDailyTitle(lang)}");
        sb.AppendLine();

        foreach (var item in items)
        {
            var maxTemp = (int)Math.Round(item.MaxTemperature);
            var minTemp = (int)Math.Round(item.MinTemperature);
            var emoji = GetWeatherEmoji(item.WeatherCode);
            var dayOfWeek = FormatDayOfWeek(item.Date.DayOfWeek, lang);
            sb.AppendLine($"{dayOfWeek} {item.Date:dd.MM}  {emoji} {maxTemp:+#;-#;0}°/{minTemp:+#;-#;0}°  💧{item.PrecipitationProbability}%");
        }

        return sb.ToString().TrimEnd();
    }

    private static string FormatHourlyTitle(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "почасовой прогноз",
        BotLanguage.English => "hourly forecast",
        _ => "погодинний прогноз"
    };

    private static string FormatDailyTitle(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "прогноз на 5 дней",
        BotLanguage.English => "5-day forecast",
        _ => "прогноз на 5 днів"
    };

    private static string FormatDayOfWeek(DayOfWeek day, BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => day switch
        {
            DayOfWeek.Monday => "Пн",
            DayOfWeek.Tuesday => "Вт",
            DayOfWeek.Wednesday => "Ср",
            DayOfWeek.Thursday => "Чт",
            DayOfWeek.Friday => "Пт",
            DayOfWeek.Saturday => "Сб",
            DayOfWeek.Sunday => "Вс",
            _ => ""
        },
        BotLanguage.English => day switch
        {
            DayOfWeek.Monday => "Mon",
            DayOfWeek.Tuesday => "Tue",
            DayOfWeek.Wednesday => "Wed",
            DayOfWeek.Thursday => "Thu",
            DayOfWeek.Friday => "Fri",
            DayOfWeek.Saturday => "Sat",
            DayOfWeek.Sunday => "Sun",
            _ => ""
        },
        _ => day switch
        {
            DayOfWeek.Monday => "Пн",
            DayOfWeek.Tuesday => "Вт",
            DayOfWeek.Wednesday => "Ср",
            DayOfWeek.Thursday => "Чт",
            DayOfWeek.Friday => "Пт",
            DayOfWeek.Saturday => "Сб",
            DayOfWeek.Sunday => "Нд",
            _ => ""
        }
    };

    private static string GetWeatherEmoji(WeatherCode code) => code switch
    {
        WeatherCode.ClearSky => "☀️",
        WeatherCode.MainlyClear => "🌤",
        WeatherCode.PartlyCloudy => "⛅",
        WeatherCode.Overcast => "☁️",
        WeatherCode.Fog or WeatherCode.DepositingRimeFog => "🌫",
        WeatherCode.LightDrizzle or WeatherCode.ModerateDrizzle or WeatherCode.DenseDrizzle => "🌦",
        WeatherCode.LightFreezingDrizzle or WeatherCode.DenseFreezingDrizzle => "🌧",
        WeatherCode.SlightRain or WeatherCode.ModerateRain or WeatherCode.HeavyRain => "🌧",
        WeatherCode.LightFreezingRain or WeatherCode.HeavyFreezingRain => "🌨",
        WeatherCode.SlightSnow or WeatherCode.ModerateSnow or WeatherCode.HeavySnow or WeatherCode.SnowGrains => "❄️",
        WeatherCode.SlightRainShowers or WeatherCode.ModerateRainShowers or WeatherCode.ViolentRainShowers => "🌧",
        WeatherCode.SlightSnowShowers or WeatherCode.HeavySnowShowers => "🌨",
        WeatherCode.Thunderstorm or WeatherCode.ThunderstormWithSlightHail or WeatherCode.ThunderstormWithHeavyHail => "⛈",
        _ => "🌡"
    };
}
