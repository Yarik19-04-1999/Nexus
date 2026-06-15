using Information.Api.Bot.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Information.Api.Bot.Services;

public class BotPollingService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly BotUpdateHandler _updateHandler;
    private readonly ILogger<BotPollingService> _logger;

    public BotPollingService(
        ITelegramBotClient botClient,
        BotUpdateHandler updateHandler,
        ILogger<BotPollingService> logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ice Age Brief bot started");
        _botClient.StartReceiving(
            _updateHandler,
            receiverOptions: new ReceiverOptions { AllowedUpdates = [] },
            cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
