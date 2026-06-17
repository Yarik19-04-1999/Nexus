using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Information.Api.Bot.Constants;
using Information.Api.Bot.Extensions;
using Information.Api.Bot.Formatters;
using Information.Api.Bot.Localization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Information.Api.Bot.Handlers;

public partial class BotUpdateHandler : IUpdateHandler
{
    private static readonly InlineKeyboardMarkup LanguageKeyboard = new(new[]
    {
        new[] { InlineKeyboardButton.WithCallbackData("🇺🇦 Українська", IceAgeBriefTelegramBotConstants.Callbacks.LangUk) },
        new[] { InlineKeyboardButton.WithCallbackData("🇷🇺 Русский",    IceAgeBriefTelegramBotConstants.Callbacks.LangRu) },
        new[] { InlineKeyboardButton.WithCallbackData("🇬🇧 English",    IceAgeBriefTelegramBotConstants.Callbacks.LangEn) }
    });

    private static readonly IReadOnlyDictionary<BotLanguage, InlineKeyboardMarkup> MainMenuKeyboards =
        Enum.GetValues<BotLanguage>().ToDictionary(lang => lang, lang => new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnWeather(lang),      IceAgeBriefTelegramBotConstants.Callbacks.WeatherMenu) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnRatesToday(lang),   IceAgeBriefTelegramBotConstants.Callbacks.RatesToday) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnRatesHistory(lang), IceAgeBriefTelegramBotConstants.Callbacks.RatesHistory) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnEpicGames(lang),    IceAgeBriefTelegramBotConstants.Callbacks.EpicGames) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnLanguage(lang),     IceAgeBriefTelegramBotConstants.Callbacks.LangMenu) }
        }));

    private static readonly IReadOnlyDictionary<BotLanguage, InlineKeyboardMarkup> BackToMenuKeyboards =
        Enum.GetValues<BotLanguage>().ToDictionary(lang => lang, lang => new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBackToMenu(lang), IceAgeBriefTelegramBotConstants.Callbacks.MenuMain) }
        }));

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BotUpdateHandler> _logger;

    public BotUpdateHandler(IServiceScopeFactory scopeFactory, ILogger<BotUpdateHandler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        try
        {
            await HandleUpdateInternalAsync(bot, update, ct);
        }
        catch (Exception ex)
        {
            LogUnhandledUpdateError(ex, update.Id);
            await TrySendErrorAsync(bot, update, ct);
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, HandleErrorSource source, CancellationToken ct)
    {
        LogPollingError(exception, source);
        return Task.CompletedTask;
    }

    private async Task HandleUpdateInternalAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Message is { From: not null, Text: not null } message)
        {
            await HandleMessageAsync(bot, message, ct);
        }
        else if (update.CallbackQuery is { Data: not null, Message: not null } query)
        {
            await bot.AnswerCallbackQuery(query.Id, cancellationToken: ct);
            await HandleCallbackAsync(bot, query, ct);
        }
    }

    private async Task HandleMessageAsync(ITelegramBotClient bot, Message message, CancellationToken ct)
    {
        var userId = message.From!.Id;
        var chatId = message.Chat.Id;
        var lang = await GetLanguage(userId, ct);

        if (message.Text == IceAgeBriefTelegramBotConstants.Commands.Start)
        {
            await bot.SendMessage(chatId,
                BotMessages.Welcome(lang) + "\n\n" + BotMessages.ChooseLanguage(lang),
                replyMarkup: LanguageKeyboard,
                cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(chatId,
                BotMessages.MainMenu(lang),
                replyMarkup: MainMenuKeyboards[lang],
                cancellationToken: ct);
        }
    }

    private async Task HandleCallbackAsync(ITelegramBotClient bot, CallbackQuery query, CancellationToken ct)
    {
        var userId = query.From.Id;
        var chatId = query.Message!.Chat.Id;
        var messageId = query.Message.MessageId;
        var data = query.Data!;

        if (data is IceAgeBriefTelegramBotConstants.Callbacks.LangRu
                 or IceAgeBriefTelegramBotConstants.Callbacks.LangUk
                 or IceAgeBriefTelegramBotConstants.Callbacks.LangEn)
        {
            await HandleSetLanguageAsync(bot, chatId, messageId, userId, data, ct);
            return;
        }

        var lang = await GetLanguage(userId, ct);

        switch (data)
        {
            case IceAgeBriefTelegramBotConstants.Callbacks.LangMenu:
                await bot.EditMessageText(chatId, messageId,
                    BotMessages.ChooseLanguage(lang),
                    replyMarkup: LanguageKeyboard,
                    cancellationToken: ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.MenuMain:
                await EditToMainMenuAsync(bot, chatId, messageId, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesToday:
                await HandleRatesTodayAsync(bot, chatId, messageId, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesHistory:
                await bot.EditMessageText(chatId, messageId,
                    BotMessages.ChooseCurrencyHistory(lang),
                    replyMarkup: BuildCurrencyHistoryKeyboard(lang),
                    cancellationToken: ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryAll:
                await HandleRatesHistoryAsync(bot, chatId, messageId, null, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryUsd:
                await HandleRatesHistoryAsync(bot, chatId, messageId, ExchangeCurrency.USD, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryEur:
                await HandleRatesHistoryAsync(bot, chatId, messageId, ExchangeCurrency.EUR, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryGbp:
                await HandleRatesHistoryAsync(bot, chatId, messageId, ExchangeCurrency.GBP, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.WeatherMenu:
                await bot.EditMessageText(chatId, messageId,
                    BotMessages.ChooseCity(lang),
                    replyMarkup: BuildCitySelectionKeyboard(lang),
                    cancellationToken: ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.EpicGames:
                await HandleEpicGamesAsync(bot, chatId, messageId, lang, ct);
                break;

            default:
                if (data.StartsWith(IceAgeBriefTelegramBotConstants.Callbacks.WeatherCityPrefix))
                {
                    await HandleCitySelectedAsync(bot, chatId, messageId, data, lang, ct);
                }
                else if (data.StartsWith(IceAgeBriefTelegramBotConstants.Callbacks.WeatherHourlyPrefix))
                {
                    var cityStr = data[IceAgeBriefTelegramBotConstants.Callbacks.WeatherHourlyPrefix.Length..];
                    if (Enum.TryParse<WeatherCity>(cityStr, out var city))
                    {
                        await HandleHourlyWeatherAsync(bot, chatId, messageId, city, lang, ct);
                    }
                }
                else if (data.StartsWith(IceAgeBriefTelegramBotConstants.Callbacks.WeatherDailyPrefix))
                {
                    var cityStr = data[IceAgeBriefTelegramBotConstants.Callbacks.WeatherDailyPrefix.Length..];
                    if (Enum.TryParse<WeatherCity>(cityStr, out var city))
                    {
                        await HandleDailyWeatherAsync(bot, chatId, messageId, city, lang, ct);
                    }
                }
                break;
        }
    }

    private async Task HandleSetLanguageAsync(ITelegramBotClient bot, long chatId, int messageId, long userId, string data, CancellationToken ct)
    {
        var lang = data.ToBotLanguage();

        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ISetUserLanguageUseCase>();
        await useCase.Execute(new SetUserLanguageInput(userId, lang), ct);

        await EditToMainMenuAsync(bot, chatId, messageId, lang, ct);
    }

    private async Task EditToMainMenuAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        await bot.EditMessageText(chatId, messageId,
            BotMessages.MainMenu(lang),
            replyMarkup: MainMenuKeyboards[lang],
            cancellationToken: ct);
    }

    private async Task HandleRatesTodayAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetExchangeRatesUseCase>();
        var data = await useCase.Execute(GetExchangeRatesInput.Instance, ct);
        var text = ExchangeRateFormatter.FormatToday(data, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboards[lang], cancellationToken: ct);
    }

    private async Task HandleRatesHistoryAsync(ITelegramBotClient bot, long chatId, int messageId, ExchangeCurrency? currency, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetExchangeRateHistoryUseCase>();
        var data = await useCase.Execute(new GetExchangeRateHistoryInput(currency), ct);
        var text = ExchangeRateFormatter.FormatHistory(data, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboards[lang], cancellationToken: ct);
    }

    private async Task HandleCitySelectedAsync(ITelegramBotClient bot, long chatId, int messageId, string data, BotLanguage lang, CancellationToken ct)
    {
        var cityStr = data[IceAgeBriefTelegramBotConstants.Callbacks.WeatherCityPrefix.Length..];
        if (!Enum.TryParse<WeatherCity>(cityStr, out var city))
        {
            return;
        }

        await bot.EditMessageText(chatId, messageId,
            BotMessages.ChooseWeatherType(lang),
            replyMarkup: BuildWeatherTypeKeyboard(city, lang),
            cancellationToken: ct);
    }

    private async Task HandleHourlyWeatherAsync(ITelegramBotClient bot, long chatId, int messageId, WeatherCity city, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetHourlyWeatherUseCase>();
        var data = await useCase.Execute(new GetWeatherInput(city), ct);
        var text = WeatherFormatter.FormatHourly(data, city, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboards[lang], cancellationToken: ct);
    }

    private async Task HandleDailyWeatherAsync(ITelegramBotClient bot, long chatId, int messageId, WeatherCity city, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetDailyWeatherUseCase>();
        var data = await useCase.Execute(new GetWeatherInput(city), ct);
        var text = WeatherFormatter.FormatDaily(data, city, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboards[lang], cancellationToken: ct);
    }

    private async Task HandleEpicGamesAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetEpicFreeGamesUseCase>();
        var data = await useCase.Execute(GetEpicFreeGamesInput.Instance, ct);

        if (data.Count == 0)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.NoEpicGames(lang),
                replyMarkup: BackToMenuKeyboards[lang],
                cancellationToken: ct);
            return;
        }

        var text = EpicGamesFormatter.Format(data, lang);
        await bot.EditMessageText(chatId, messageId, text,
            parseMode: ParseMode.Html,
            replyMarkup: BackToMenuKeyboards[lang],
            cancellationToken: ct);
    }

    private async Task TrySendErrorAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        try
        {
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
            if (chatId is null)
            {
                return;
            }

            await bot.SendMessage(chatId.Value,
                BotMessages.UnexpectedError(BotLanguage.Ukrainian),
                cancellationToken: ct);
        }
        catch { }
    }

    private async Task<BotLanguage> GetLanguage(long userId, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUserLanguageUseCase>();
        return await useCase.Execute(new GetUserLanguageInput(userId), ct);
    }

    private static InlineKeyboardMarkup BuildCurrencyHistoryKeyboard(BotLanguage lang) =>
        new(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("🇺🇸 USD", IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryUsd) },
            new[] { InlineKeyboardButton.WithCallbackData("🇪🇺 EUR", IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryEur) },
            new[] { InlineKeyboardButton.WithCallbackData("🇬🇧 GBP", IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryGbp) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnAllCurrencies(lang), IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryAll) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBackToMenu(lang),    IceAgeBriefTelegramBotConstants.Callbacks.MenuMain) }
        });

    private static InlineKeyboardMarkup BuildCitySelectionKeyboard(BotLanguage lang)
    {
        var rows = Enum.GetValues<WeatherCity>()
            .Select(city => new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    BotMessages.GetCityName(city, lang),
                    IceAgeBriefTelegramBotConstants.Callbacks.WeatherCityPrefix + city)
            })
            .Append(new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBackToMenu(lang), IceAgeBriefTelegramBotConstants.Callbacks.MenuMain) })
            .ToArray();

        return new InlineKeyboardMarkup(rows);
    }

    private static InlineKeyboardMarkup BuildWeatherTypeKeyboard(WeatherCity city, BotLanguage lang) =>
        new(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnHourly(lang), IceAgeBriefTelegramBotConstants.Callbacks.WeatherHourlyPrefix + city) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnDaily(lang),  IceAgeBriefTelegramBotConstants.Callbacks.WeatherDailyPrefix + city) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBack(lang),   IceAgeBriefTelegramBotConstants.Callbacks.WeatherMenu) }
        });

    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled error processing update {UpdateId}")]
    private partial void LogUnhandledUpdateError(Exception ex, int updateId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Telegram polling error — source: {Source}")]
    private partial void LogPollingError(Exception exception, HandleErrorSource source);
}
