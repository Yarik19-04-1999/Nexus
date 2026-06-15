using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Information.Api.Bot.Constants;
using Information.Api.Bot.Extensions;
using Information.Api.Bot.Formatters;
using Information.Api.Bot.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Information.Api.Bot.Handlers;

public class BotUpdateHandler : IUpdateHandler
{
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
            _logger.LogError(ex, "Unhandled error processing update {UpdateId}", update.Id);
            await TrySendErrorAsync(bot, update, ct);
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, HandleErrorSource source, CancellationToken ct)
    {
        _logger.LogError(exception, "Telegram polling error — source: {Source}", source);
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

        if (message.Text == IceAgeBriefTelegramBotConstants.Commands.Start)
        {
            var lang = await GetLanguage(userId, ct);
            await SendLanguageSelectionAsync(bot, chatId, lang, ct);
        }
        else
        {
            var lang = await GetLanguage(userId, ct);
            await SendMainMenuAsync(bot, chatId, lang, ct);
        }
    }

    private async Task HandleCallbackAsync(ITelegramBotClient bot, CallbackQuery query, CancellationToken ct)
    {
        var userId = query.From.Id;
        var chatId = query.Message!.Chat.Id;
        var messageId = query.Message.MessageId;
        var data = query.Data!;

        if (data is IceAgeBriefTelegramBotConstants.Callbacks.LangRu or IceAgeBriefTelegramBotConstants.Callbacks.LangUk or IceAgeBriefTelegramBotConstants.Callbacks.LangEn)
        {
            await HandleSetLanguageAsync(bot, chatId, messageId, userId, data, ct);
            return;
        }

        var lang = await GetLanguage(userId, ct);

        switch (data)
        {
            case IceAgeBriefTelegramBotConstants.Callbacks.LangMenu:
                await EditToLanguageSelectionAsync(bot, chatId, messageId, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.MenuMain:
                await EditToMainMenuAsync(bot, chatId, messageId, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesToday:
                await HandleRatesTodayAsync(bot, chatId, messageId, lang, ct);
                break;

            case IceAgeBriefTelegramBotConstants.Callbacks.RatesHistory:
                await EditToCurrencyHistorySelectionAsync(bot, chatId, messageId, lang, ct);
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
                await EditToCitySelectionAsync(bot, chatId, messageId, lang, ct);
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

    private async Task SendLanguageSelectionAsync(ITelegramBotClient bot, long chatId, BotLanguage lang, CancellationToken ct)
    {
        var keyboard = BuildLanguageKeyboard();
        await bot.SendMessage(chatId,
            BotMessages.Welcome(lang) + "\n\n" + BotMessages.ChooseLanguage(lang),
            replyMarkup: keyboard,
            cancellationToken: ct);
    }

    private async Task EditToLanguageSelectionAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        await bot.EditMessageText(chatId, messageId,
            BotMessages.ChooseLanguage(lang),
            replyMarkup: BuildLanguageKeyboard(),
            cancellationToken: ct);
    }

    private async Task HandleSetLanguageAsync(ITelegramBotClient bot, long chatId, int messageId, long userId, string data, CancellationToken ct)
    {
        var lang = data.ToBotLanguage();

        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ISetUserLanguageUseCase>();
        await useCase.Execute(new SetUserLanguageInput(userId, lang), ct);

        await bot.EditMessageText(chatId, messageId,
            BotMessages.LanguageSet(lang),
            cancellationToken: ct);
        await SendMainMenuAsync(bot, chatId, lang, ct);
    }

    private static InlineKeyboardMarkup BuildLanguageKeyboard() =>
        new(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("🇺🇦 Українська", IceAgeBriefTelegramBotConstants.Callbacks.LangUk) },
            new[] { InlineKeyboardButton.WithCallbackData("🇷🇺 Русский", IceAgeBriefTelegramBotConstants.Callbacks.LangRu) },
            new[] { InlineKeyboardButton.WithCallbackData("🇬🇧 English", IceAgeBriefTelegramBotConstants.Callbacks.LangEn) }
        });

    private async Task SendMainMenuAsync(ITelegramBotClient bot, long chatId, BotLanguage lang, CancellationToken ct)
    {
        await bot.SendMessage(chatId,
            BotMessages.MainMenu(lang),
            replyMarkup: BuildMainMenuKeyboard(lang),
            cancellationToken: ct);
    }

    private async Task EditToMainMenuAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        await bot.EditMessageText(chatId, messageId,
            BotMessages.MainMenu(lang),
            replyMarkup: BuildMainMenuKeyboard(lang),
            cancellationToken: ct);
    }

    private static InlineKeyboardMarkup BuildMainMenuKeyboard(BotLanguage lang) =>
        new(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnWeather(lang), IceAgeBriefTelegramBotConstants.Callbacks.WeatherMenu) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnRatesToday(lang), IceAgeBriefTelegramBotConstants.Callbacks.RatesToday) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnRatesHistory(lang), IceAgeBriefTelegramBotConstants.Callbacks.RatesHistory) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnEpicGames(lang), IceAgeBriefTelegramBotConstants.Callbacks.EpicGames) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnLanguage(lang), IceAgeBriefTelegramBotConstants.Callbacks.LangMenu) }
        });

    private async Task HandleRatesTodayAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetExchangeRatesUseCase>();
        var result = await useCase.Execute(new GetExchangeRatesInput(), ct);

        if (result.HasError)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.UnexpectedError(lang),
                replyMarkup: BackToMenuKeyboard(lang),
                cancellationToken: ct);
            return;
        }

        var text = ExchangeRateFormatter.FormatToday(result.Data, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboard(lang), cancellationToken: ct);
    }

    private async Task EditToCurrencyHistorySelectionAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("🇺🇸 USD", IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryUsd) },
            new[] { InlineKeyboardButton.WithCallbackData("🇪🇺 EUR", IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryEur) },
            new[] { InlineKeyboardButton.WithCallbackData("🇬🇧 GBP", IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryGbp) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnAllCurrencies(lang), IceAgeBriefTelegramBotConstants.Callbacks.RatesHistoryAll) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBackToMenu(lang), IceAgeBriefTelegramBotConstants.Callbacks.MenuMain) }
        });

        await bot.EditMessageText(chatId, messageId,
            BotMessages.ChooseCurrencyHistory(lang),
            replyMarkup: keyboard,
            cancellationToken: ct);
    }

    private async Task HandleRatesHistoryAsync(ITelegramBotClient bot, long chatId, int messageId, ExchangeCurrency? currency, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetExchangeRateHistoryUseCase>();
        var result = await useCase.Execute(new GetExchangeRateHistoryInput(currency), ct);

        if (result.HasError)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.UnexpectedError(lang),
                replyMarkup: BackToMenuKeyboard(lang),
                cancellationToken: ct);
            return;
        }

        var text = ExchangeRateFormatter.FormatHistory(result.Data, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboard(lang), cancellationToken: ct);
    }

    // ── Weather ───────────────────────────────────────────────────────────────

    private async Task EditToCitySelectionAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
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

        await bot.EditMessageText(chatId, messageId,
            BotMessages.ChooseCity(lang),
            replyMarkup: new InlineKeyboardMarkup(rows),
            cancellationToken: ct);
    }

    private async Task HandleCitySelectedAsync(ITelegramBotClient bot, long chatId, int messageId, string data, BotLanguage lang, CancellationToken ct)
    {
        var cityStr = data[IceAgeBriefTelegramBotConstants.Callbacks.WeatherCityPrefix.Length..];
        if (!Enum.TryParse<WeatherCity>(cityStr, out var city))
        {
            return;
        }

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnHourly(lang), IceAgeBriefTelegramBotConstants.Callbacks.WeatherHourlyPrefix + city) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnDaily(lang), IceAgeBriefTelegramBotConstants.Callbacks.WeatherDailyPrefix + city) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBack(lang), IceAgeBriefTelegramBotConstants.Callbacks.WeatherMenu) }
        });

        await bot.EditMessageText(chatId, messageId,
            BotMessages.ChooseWeatherType(lang),
            replyMarkup: keyboard,
            cancellationToken: ct);
    }

    private async Task HandleHourlyWeatherAsync(ITelegramBotClient bot, long chatId, int messageId, WeatherCity city, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetHourlyWeatherUseCase>();
        var result = await useCase.Execute(new GetWeatherInput(city), ct);

        if (result.HasError)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.UnexpectedError(lang),
                replyMarkup: BackToMenuKeyboard(lang),
                cancellationToken: ct);
            return;
        }

        var text = WeatherFormatter.FormatHourly(result.Data, city, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboard(lang), cancellationToken: ct);
    }

    private async Task HandleDailyWeatherAsync(ITelegramBotClient bot, long chatId, int messageId, WeatherCity city, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetDailyWeatherUseCase>();
        var result = await useCase.Execute(new GetWeatherInput(city), ct);

        if (result.HasError)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.UnexpectedError(lang),
                replyMarkup: BackToMenuKeyboard(lang),
                cancellationToken: ct);
            return;
        }

        var text = WeatherFormatter.FormatDaily(result.Data, city, lang);
        await bot.EditMessageText(chatId, messageId, text, replyMarkup: BackToMenuKeyboard(lang), cancellationToken: ct);
    }

    private async Task HandleEpicGamesAsync(ITelegramBotClient bot, long chatId, int messageId, BotLanguage lang, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetEpicFreeGamesUseCase>();
        var result = await useCase.Execute(new GetEpicFreeGamesInput(), ct);

        if (result.HasError)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.UnexpectedError(lang),
                replyMarkup: BackToMenuKeyboard(lang),
                cancellationToken: ct);
            return;
        }

        if (result.Data.Count == 0)
        {
            await bot.EditMessageText(chatId, messageId,
                BotMessages.NoEpicGames(lang),
                replyMarkup: BackToMenuKeyboard(lang),
                cancellationToken: ct);
            return;
        }

        var text = EpicGamesFormatter.Format(result.Data, lang);
        await bot.EditMessageText(chatId, messageId, text,
            parseMode: ParseMode.Html,
            replyMarkup: BackToMenuKeyboard(lang),
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
        catch
        {
            // best-effort, ignore
        }
    }

    private async Task<BotLanguage> GetLanguage(long userId, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUserLanguageUseCase>();
        var result = await useCase.Execute(new GetUserLanguageInput(userId), ct);
        return result.Data;
    }

    private static InlineKeyboardMarkup BackToMenuKeyboard(BotLanguage lang) =>
        new(new[] { new[] { InlineKeyboardButton.WithCallbackData(BotMessages.BtnBackToMenu(lang), IceAgeBriefTelegramBotConstants.Callbacks.MenuMain) } });

}
