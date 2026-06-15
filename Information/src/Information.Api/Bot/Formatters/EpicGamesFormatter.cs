using System.Net;
using System.Text;
using Information.Application.Enums;
using Information.Application.Models;
using Information.Api.Bot.Constants;
using Information.Api.Bot.Localization;

namespace Information.Api.Bot.Formatters;

public static class EpicGamesFormatter
{
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
                var description = game.Description.Length > 120
                    ? game.Description[..117] + "..."
                    : game.Description;
                sb.AppendLine(WebUtility.HtmlEncode(description));
            }

            sb.AppendLine($"🗓 {BotMessages.FreeUntilLabel(lang)}: {game.FreeUntil.ToString(IceAgeBriefTelegramBotConstants.DisplayDateFormat)}");
            sb.AppendLine($"🔗 <a href=\"{game.StoreUrl}\">{BotMessages.GetLabel(lang)}</a>");
        }

        return sb.ToString().TrimEnd();
    }
}
