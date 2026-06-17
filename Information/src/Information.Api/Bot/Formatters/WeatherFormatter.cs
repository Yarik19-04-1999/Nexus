using System.Text;
using Information.Application.Enums;
using Information.Application.Models;
using Information.Api.Bot.Localization;

namespace Information.Api.Bot.Formatters;

public static class WeatherFormatter
{
    public static string FormatHourly(IReadOnlyList<HourlyWeather> items, WeatherCity city, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"🌤 {BotMessages.GetCityName(city, lang)} — {WeatherMessages.HourlyTitle(lang)}");
        sb.AppendLine();

        foreach (var item in items)
        {
            var temp = (int)Math.Round(item.Temperature);
            var emoji = GetWeatherEmoji(item.WeatherCode);
            sb.AppendLine($"{item.Time:HH:mm}  {emoji} {temp:+#;-#;0}°  💧{item.PrecipitationProbability}%");
        }

        return sb.ToString().TrimEnd();
    }

    public static string FormatDaily(IReadOnlyList<DailyWeather> items, WeatherCity city, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"📅 {BotMessages.GetCityName(city, lang)} — {WeatherMessages.DailyTitle(lang)}");
        sb.AppendLine();

        foreach (var item in items)
        {
            var maxTemp = (int)Math.Round(item.MaxTemperature);
            var minTemp = (int)Math.Round(item.MinTemperature);
            var emoji = GetWeatherEmoji(item.WeatherCode);
            var dayOfWeek = WeatherMessages.AbbreviatedDayOfWeek(item.Date.DayOfWeek, lang);
            sb.AppendLine($"{dayOfWeek} {item.Date:dd.MM}  {emoji} {maxTemp:+#;-#;0}°/{minTemp:+#;-#;0}°  💧{item.PrecipitationProbability}%");
        }

        return sb.ToString().TrimEnd();
    }

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
