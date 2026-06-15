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
    public static IServiceCollection AddIceAgeBriefTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BotOptions>()
            .BindConfiguration("IceAgeBrief")
            .WithValidator<BotOptions, BotOptionsValidator>()
            .ValidateOnStart();

        services.AddSingleton<ITelegramBotClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<BotOptions>>().Value;
            return new TelegramBotClient(options.Token);
        });

        services.AddSingleton<BotUpdateHandler>();
        services.AddHostedService<BotPollingService>();

        return services;
    }
}
