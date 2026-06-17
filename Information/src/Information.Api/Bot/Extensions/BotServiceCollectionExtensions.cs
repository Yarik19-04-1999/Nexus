using Information.Api.Bot.Constants;
using Information.Api.Bot.Handlers;
using Information.Api.Bot.Options;
using Information.Api.Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Extensions;
using Telegram.Bot;

namespace Information.Api.Bot.Extensions;

public static class BotServiceCollectionExtensions
{
    public static IServiceCollection AddIceAgeBriefTelegramBot(this IServiceCollection services)
        => services
            .AddOptions<BotOptions>()
            .BindConfiguration(ConfigurationConstants.IceAgeBriefSection)
            .WithValidator<BotOptions, BotOptionsValidator>()
            .ValidateOnStart().Services
            .AddSingleton<ITelegramBotClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BotOptions>>().Value;
                return new TelegramBotClient(options.Token);
            })
            .AddSingleton<BotUpdateHandler>()
            .AddHostedService<BotPollingService>();
}
