using System.Globalization;
using Information.Application.Enums;
using Nexus.Application.Core.Cultures;

namespace Information.Application.Extensions;

public static class BotLanguageExtensions
{
    public static CultureInfo ToCulture(this BotLanguage lang) => lang switch
    {
        BotLanguage.Russian => AppCultures.Russian,
        BotLanguage.English => AppCultures.English,
        _ => AppCultures.Ukrainian
    };
}
