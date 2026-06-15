using Information.Api.Bot.Constants;
using Information.Application.Enums;

namespace Information.Api.Bot.Extensions;

public static class BotLanguageExtensions
{
    public static BotLanguage ToBotLanguage(this string callbackData) => callbackData switch
    {
        IceAgeBriefTelegramBotConstants.Callbacks.LangRu => BotLanguage.Russian,
        IceAgeBriefTelegramBotConstants.Callbacks.LangEn => BotLanguage.English,
        _ => BotLanguage.Ukrainian
    };
}
