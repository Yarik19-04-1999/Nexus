namespace Information.Api.Bot.Constants;

public static class IceAgeBriefTelegramBotConstants
{
    public const string DisplayDateFormat = "dd.MM.yyyy";

    public static class Commands
    {
        public const string Start = "/start";
    }

    public static class Callbacks
    {
        public const string LangMenu = "lang:menu";
        public const string LangRu = "lang:ru";
        public const string LangUk = "lang:uk";
        public const string LangEn = "lang:en";

        public const string MenuMain = "menu:main";

        public const string RatesToday = "rates:today";
        public const string RatesHistory = "rates:history";
        public const string RatesHistoryAll = "rates:history:all";
        public const string RatesHistoryUsd = "rates:history:USD";
        public const string RatesHistoryEur = "rates:history:EUR";
        public const string RatesHistoryGbp = "rates:history:GBP";

        public const string WeatherMenu = "weather:menu";
        public const string WeatherCityPrefix = "weather:city:";
        public const string WeatherHourlyPrefix = "weather:hourly:";
        public const string WeatherDailyPrefix = "weather:daily:";

        public const string EpicGames = "epic:games";
    }
}
