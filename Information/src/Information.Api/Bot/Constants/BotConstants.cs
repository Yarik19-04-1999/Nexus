namespace Information.Api.Bot.Constants;

internal static class BotConstants
{
    internal static class Commands
    {
        internal const string Start = "/start";
    }

    internal static class Callbacks
    {
        internal const string LangMenu = "lang:menu";
        internal const string LangRu = "lang:ru";
        internal const string LangUk = "lang:uk";
        internal const string LangEn = "lang:en";

        internal const string MenuMain = "menu:main";

        internal const string RatesToday = "rates:today";
        internal const string RatesHistory = "rates:history";
        internal const string RatesHistoryAll = "rates:history:all";
        internal const string RatesHistoryUsd = "rates:history:USD";
        internal const string RatesHistoryEur = "rates:history:EUR";
        internal const string RatesHistoryGbp = "rates:history:GBP";

        internal const string WeatherMenu = "weather:menu";
        internal const string WeatherCityPrefix = "weather:city:";
        internal const string WeatherHourlyPrefix = "weather:hourly:";
        internal const string WeatherDailyPrefix = "weather:daily:";

        internal const string EpicGames = "epic:games";
    }
}
