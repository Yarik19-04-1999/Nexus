using System.Net;
using System.Text;
using Information.Application.Enums;
using Information.Application.Models;
using Information.Api.Bot.Constants;
using Information.Api.Bot.Localization;
using Nexus.Application.Core.Utils;

namespace Information.Api.Bot.Formatters;

public static class EpicGamesFormatter
{
    private const int DescriptionMaxLength = 120;

    public static string Format(IReadOnlyList<EpicGame> games, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine(BotMessages.EpicGamesHeader(lang));

        foreach (var game in games)
        {
            sb.AppendLine();
            sb.AppendLine($"<b>{WebUtility.HtmlEncode(game.Title)}</b>");

            if (!string.IsNullOrWhiteSpace(game.Description))
            {
                sb.AppendLine(WebUtility.HtmlEncode(StringUtils.TruncateWithEllipsis(game.Description, DescriptionMaxLength)));
            }

            sb.AppendLine($"🗓 {BotMessages.FreeUntilLabel(lang)}: {game.FreeUntil.ToString(IceAgeBriefTelegramBotConstants.DisplayDateFormat)}");
            sb.AppendLine($"🔗 <a href=\"{game.StoreUrl}\">{BotMessages.GetLabel(lang)}</a>");
        }

        return sb.ToString().TrimEnd();
    }
}
