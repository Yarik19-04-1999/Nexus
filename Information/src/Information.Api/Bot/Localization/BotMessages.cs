using Information.Application.Enums;

namespace Information.Api.Bot.Localization;

internal static class BotMessages
{
    internal static string Welcome(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Добро пожаловать! Я бот Ice Age Brief — курсы валют, погода и бесплатные игры Epic Games.",
        BotLanguage.English => "Welcome! I'm Ice Age Brief — exchange rates, weather and free Epic Games.",
        _ => "Ласкаво просимо! Я бот Ice Age Brief — курси валют, погода та безкоштовні ігри Epic Games."
    };

    internal static string ChooseLanguage(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите язык:",
        BotLanguage.English => "Choose your language:",
        _ => "Оберіть мову:"
    };

    internal static string LanguageSet(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "✅ Язык установлен.",
        BotLanguage.English => "✅ Language set.",
        _ => "✅ Мову встановлено."
    };

    internal static string MainMenu(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Главное меню:",
        BotLanguage.English => "Main menu:",
        _ => "Головне меню:"
    };

    internal static string BtnWeather(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🌤 Погода",
        BotLanguage.English => "🌤 Weather",
        _ => "🌤 Погода"
    };

    internal static string BtnRatesToday(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "💱 Курс сегодня",
        BotLanguage.English => "💱 Rates today",
        _ => "💱 Курс сьогодні"
    };

    internal static string BtnRatesHistory(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📈 Курс за год",
        BotLanguage.English => "📈 Rates (year)",
        _ => "📈 Курс за рік"
    };

    internal static string BtnEpicGames(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Epic Games",
        BotLanguage.English => "🎮 Epic Games",
        _ => "🎮 Epic Games"
    };

    internal static string BtnLanguage(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🌐 Язык",
        BotLanguage.English => "🌐 Language",
        _ => "🌐 Мова"
    };

    internal static string BtnBack(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "« Назад",
        BotLanguage.English => "« Back",
        _ => "« Назад"
    };

    internal static string BtnBackToMenu(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "« Главное меню",
        BotLanguage.English => "« Main menu",
        _ => "« Головне меню"
    };

    internal static string ChooseCity(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите город:",
        BotLanguage.English => "Choose a city:",
        _ => "Оберіть місто:"
    };

    internal static string ChooseWeatherType(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите тип прогноза:",
        BotLanguage.English => "Choose forecast type:",
        _ => "Оберіть тип прогнозу:"
    };

    internal static string BtnHourly(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🕐 Почасовой (24 ч)",
        BotLanguage.English => "🕐 Hourly (24 h)",
        _ => "🕐 Погодинно (24 год)"
    };

    internal static string BtnDaily(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📅 Ежедневный (5 дней)",
        BotLanguage.English => "📅 Daily (5 days)",
        _ => "📅 Щоденно (5 днів)"
    };

    internal static string ChooseCurrencyHistory(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите валюту для истории за год:",
        BotLanguage.English => "Choose a currency for yearly history:",
        _ => "Оберіть валюту для річної історії:"
    };

    internal static string BtnAllCurrencies(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Все валюты",
        BotLanguage.English => "All currencies",
        _ => "Всі валюти"
    };

    internal static string NoEpicGames(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Сейчас нет бесплатных игр.",
        BotLanguage.English => "🎮 No free games available right now.",
        _ => "🎮 Зараз немає безкоштовних ігор."
    };

    internal static string EpicGamesHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Бесплатные игры Epic Games:",
        BotLanguage.English => "🎮 Free Epic Games:",
        _ => "🎮 Безкоштовні ігри Epic Games:"
    };

    internal static string FreeUntilLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "До",
        BotLanguage.English => "Until",
        _ => "До"
    };

    internal static string GetLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Получить",
        BotLanguage.English => "Get",
        _ => "Отримати"
    };

    internal static string RatesTodayHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "💱 Курс НБУ",
        BotLanguage.English => "💱 NBU Exchange Rates",
        _ => "💱 Курс НБУ"
    };

    internal static string RatesHistoryHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📈 Курс за год",
        BotLanguage.English => "📈 Yearly rate history",
        _ => "📈 Курс за рік"
    };

    internal static string YearAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Год назад",
        BotLanguage.English => "Year ago",
        _ => "Рік тому"
    };

    internal static string TodayLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Сегодня",
        BotLanguage.English => "Today",
        _ => "Сьогодні"
    };

    internal static string UnexpectedError(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "⚠️ Неожиданная ошибка. Попробуйте позже.",
        BotLanguage.English => "⚠️ Unexpected error. Please try again later.",
        _ => "⚠️ Несподівана помилка. Спробуйте пізніше."
    };

    internal static string GetCityName(WeatherCity city, BotLanguage lang) => (city, lang) switch
    {
        (WeatherCity.Kharkiv, BotLanguage.Russian) => "Харьков",
        (WeatherCity.Kharkiv, BotLanguage.English) => "Kharkiv",
        (WeatherCity.Kharkiv, _) => "Харків",
        (WeatherCity.Kyiv, BotLanguage.Russian) => "Киев",
        (WeatherCity.Kyiv, BotLanguage.English) => "Kyiv",
        (WeatherCity.Kyiv, _) => "Київ",
        (WeatherCity.Odesa, BotLanguage.Russian) => "Одесса",
        (WeatherCity.Odesa, BotLanguage.English) => "Odesa",
        (WeatherCity.Odesa, _) => "Одеса",
        (WeatherCity.Lviv, BotLanguage.Russian) => "Львов",
        (WeatherCity.Lviv, BotLanguage.English) => "Lviv",
        (WeatherCity.Lviv, _) => "Львів",
        _ => city.ToString()
    };
}
