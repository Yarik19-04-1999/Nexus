using Information.Application.Enums;

namespace Information.Api.Bot.Localization;

public static class BotMessages
{
    public static string Welcome(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Добро пожаловать! Я бот Ice Age Brief — курсы валют, погода и бесплатные игры Epic Games.",
        BotLanguage.English => "Welcome! I'm Ice Age Brief — exchange rates, weather and free Epic Games.",
        _ => "Ласкаво просимо! Я бот Ice Age Brief — курси валют, погода та безкоштовні ігри Epic Games."
    };

    public static string ChooseLanguage(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите язык:",
        BotLanguage.English => "Choose your language:",
        _ => "Оберіть мову:"
    };

    public static string MainMenu(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Главное меню:",
        BotLanguage.English => "Main menu:",
        _ => "Головне меню:"
    };

    public static string BtnWeather(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🌤 Погода",
        BotLanguage.English => "🌤 Weather",
        _ => "🌤 Погода"
    };

    public static string BtnRatesToday(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "💱 Курс сегодня",
        BotLanguage.English => "💱 Rates today",
        _ => "💱 Курс сьогодні"
    };

    public static string BtnRatesHistory(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📈 Курс за год",
        BotLanguage.English => "📈 Rates (year)",
        _ => "📈 Курс за рік"
    };

    public static string BtnEpicGames(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Epic Games",
        BotLanguage.English => "🎮 Epic Games",
        _ => "🎮 Epic Games"
    };

    public static string BtnLanguage(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🌐 Язык",
        BotLanguage.English => "🌐 Language",
        _ => "🌐 Мова"
    };

    public static string BtnBack(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "« Назад",
        BotLanguage.English => "« Back",
        _ => "« Назад"
    };

    public static string BtnBackToMenu(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "« Главное меню",
        BotLanguage.English => "« Main menu",
        _ => "« Головне меню"
    };

    public static string ChooseCity(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите город:",
        BotLanguage.English => "Choose a city:",
        _ => "Оберіть місто:"
    };

    public static string ChooseWeatherType(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите тип прогноза:",
        BotLanguage.English => "Choose forecast type:",
        _ => "Оберіть тип прогнозу:"
    };

    public static string BtnHourly(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🕐 Почасовой (24 ч)",
        BotLanguage.English => "🕐 Hourly (24 h)",
        _ => "🕐 Погодинно (24 год)"
    };

    public static string BtnDaily(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📅 Ежедневный (5 дней)",
        BotLanguage.English => "📅 Daily (5 days)",
        _ => "📅 Щоденно (5 днів)"
    };

    public static string ChooseCurrencyHistory(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите валюту для истории за год:",
        BotLanguage.English => "Choose a currency for yearly history:",
        _ => "Оберіть валюту для річної історії:"
    };

    public static string BtnAllCurrencies(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Все валюты",
        BotLanguage.English => "All currencies",
        _ => "Всі валюти"
    };

    public static string NoEpicGames(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Сейчас нет бесплатных игр.",
        BotLanguage.English => "🎮 No free games available right now.",
        _ => "🎮 Зараз немає безкоштовних ігор."
    };

    public static string EpicGamesHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Бесплатные игры Epic Games:",
        BotLanguage.English => "🎮 Free Epic Games:",
        _ => "🎮 Безкоштовні ігри Epic Games:"
    };

    public static string FreeUntilLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "До",
        BotLanguage.English => "Until",
        _ => "До"
    };

    public static string GetLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Получить",
        BotLanguage.English => "Get",
        _ => "Отримати"
    };

    public static string RatesTodayHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "💱 Курс НБУ",
        BotLanguage.English => "💱 NBU Exchange Rates",
        _ => "💱 Курс НБУ"
    };

    public static string RatesHistoryHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📈 Курс за год",
        BotLanguage.English => "📈 Yearly rate history",
        _ => "📈 Курс за рік"
    };

    public static string YesterdayLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Вчера",
        BotLanguage.English => "Yesterday",
        _ => "Вчора"
    };

    public static string WeekAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Неделю назад",
        BotLanguage.English => "Week ago",
        _ => "Тиждень тому"
    };

    public static string MonthAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Месяц назад",
        BotLanguage.English => "Month ago",
        _ => "Місяць тому"
    };

    public static string YearAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Год назад",
        BotLanguage.English => "Year ago",
        _ => "Рік тому"
    };

    public static string UnexpectedError(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "⚠️ Неожиданная ошибка. Попробуйте позже.",
        BotLanguage.English => "⚠️ Unexpected error. Please try again later.",
        _ => "⚠️ Несподівана помилка. Спробуйте пізніше."
    };

    public static string GetCityName(WeatherCity city, BotLanguage lang) => (city, lang) switch
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
