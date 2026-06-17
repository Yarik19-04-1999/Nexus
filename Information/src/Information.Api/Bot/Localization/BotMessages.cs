using Information.Application.Enums;

namespace Information.Api.Bot.Localization;

public static class BotMessages
{
    public static string Welcome(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Добро пожаловать! Я бот Ice Age Brief — курсы валют, погода и бесплатные игры Epic Games.",
        BotLanguage.Ukrainian => "Ласкаво просимо! Я бот Ice Age Brief — курси валют, погода та безкоштовні ігри Epic Games.",
        BotLanguage.English => "Welcome! I'm Ice Age Brief — exchange rates, weather and free Epic Games.",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string ChooseLanguage(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите язык:",
        BotLanguage.Ukrainian => "Оберіть мову:",
        BotLanguage.English => "Choose your language:",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string MainMenu(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Главное меню:",
        BotLanguage.Ukrainian => "Головне меню:",
        BotLanguage.English => "Main menu:",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnWeather(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🌤 Погода",
        BotLanguage.Ukrainian => "🌤 Погода",
        BotLanguage.English => "🌤 Weather",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnRatesToday(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "💱 Курс сегодня",
        BotLanguage.Ukrainian => "💱 Курс сьогодні",
        BotLanguage.English => "💱 Rates today",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnRatesHistory(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📈 Курс за год",
        BotLanguage.Ukrainian => "📈 Курс за рік",
        BotLanguage.English => "📈 Rates (year)",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnEpicGames(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Epic Games",
        BotLanguage.Ukrainian => "🎮 Epic Games",
        BotLanguage.English => "🎮 Epic Games",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnLanguage(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🌐 Язык",
        BotLanguage.Ukrainian => "🌐 Мова",
        BotLanguage.English => "🌐 Language",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnBack(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "« Назад",
        BotLanguage.Ukrainian => "« Назад",
        BotLanguage.English => "« Back",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnBackToMenu(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "« Главное меню",
        BotLanguage.Ukrainian => "« Головне меню",
        BotLanguage.English => "« Main menu",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string ChooseCity(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите город:",
        BotLanguage.Ukrainian => "Оберіть місто:",
        BotLanguage.English => "Choose a city:",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string ChooseWeatherType(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите тип прогноза:",
        BotLanguage.Ukrainian => "Оберіть тип прогнозу:",
        BotLanguage.English => "Choose forecast type:",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnHourly(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🕐 Почасовой (24 ч)",
        BotLanguage.Ukrainian => "🕐 Погодинно (24 год)",
        BotLanguage.English => "🕐 Hourly (24 h)",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnDaily(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📅 Ежедневный (5 дней)",
        BotLanguage.Ukrainian => "📅 Щоденно (5 днів)",
        BotLanguage.English => "📅 Daily (5 days)",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string ChooseCurrencyHistory(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Выберите валюту для истории за год:",
        BotLanguage.Ukrainian => "Оберіть валюту для річної історії:",
        BotLanguage.English => "Choose a currency for yearly history:",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string BtnAllCurrencies(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Все валюты",
        BotLanguage.Ukrainian => "Всі валюти",
        BotLanguage.English => "All currencies",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string NoEpicGames(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Сейчас нет бесплатных игр.",
        BotLanguage.Ukrainian => "🎮 Зараз немає безкоштовних ігор.",
        BotLanguage.English => "🎮 No free games available right now.",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string EpicGamesHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "🎮 Бесплатные игры Epic Games:",
        BotLanguage.Ukrainian => "🎮 Безкоштовні ігри Epic Games:",
        BotLanguage.English => "🎮 Free Epic Games:",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string FreeUntilLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "До",
        BotLanguage.Ukrainian => "До",
        BotLanguage.English => "Until",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string GetLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Получить",
        BotLanguage.Ukrainian => "Отримати",
        BotLanguage.English => "Get",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string RatesTodayHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "💱 Курс НБУ",
        BotLanguage.Ukrainian => "💱 Курс НБУ",
        BotLanguage.English => "💱 NBU Exchange Rates",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string RatesHistoryHeader(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "📈 Курс за год",
        BotLanguage.Ukrainian => "📈 Курс за рік",
        BotLanguage.English => "📈 Yearly rate history",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string YesterdayLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Вчера",
        BotLanguage.Ukrainian => "Вчора",
        BotLanguage.English => "Yesterday",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string WeekAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Неделю назад",
        BotLanguage.Ukrainian => "Тиждень тому",
        BotLanguage.English => "Week ago",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string MonthAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Месяц назад",
        BotLanguage.Ukrainian => "Місяць тому",
        BotLanguage.English => "Month ago",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string YearAgoLabel(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "Год назад",
        BotLanguage.Ukrainian => "Рік тому",
        BotLanguage.English => "Year ago",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string UnexpectedError(BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => "⚠️ Неожиданная ошибка. Попробуйте позже.",
        BotLanguage.Ukrainian => "⚠️ Несподівана помилка. Спробуйте пізніше.",
        BotLanguage.English => "⚠️ Unexpected error. Please try again later.",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
    };

    public static string GetCityName(WeatherCity city, BotLanguage lang) => (city, lang) switch
    {
        (WeatherCity.Kharkiv, BotLanguage.Russian) => "Харьков",
        (WeatherCity.Kharkiv, BotLanguage.Ukrainian) => "Харків",
        (WeatherCity.Kharkiv, BotLanguage.English) => "Kharkiv",
        (WeatherCity.Kyiv, BotLanguage.Russian) => "Киев",
        (WeatherCity.Kyiv, BotLanguage.Ukrainian) => "Київ",
        (WeatherCity.Kyiv, BotLanguage.English) => "Kyiv",
        (WeatherCity.Odesa, BotLanguage.Russian) => "Одесса",
        (WeatherCity.Odesa, BotLanguage.Ukrainian) => "Одеса",
        (WeatherCity.Odesa, BotLanguage.English) => "Odesa",
        (WeatherCity.Lviv, BotLanguage.Russian) => "Львов",
        (WeatherCity.Lviv, BotLanguage.Ukrainian) => "Львів",
        (WeatherCity.Lviv, BotLanguage.English) => "Lviv",
        _ => throw new ArgumentOutOfRangeException($"No localization for city={city}, lang={lang}.")
    };
}
