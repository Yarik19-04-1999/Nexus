using System.Text;
using Information.Application.Enums;
using Information.Application.Models;
using Information.Api.Bot.Localization;

namespace Information.Api.Bot.Formatters;

internal static class EpicGamesFormatter
{
    internal static string Format(IReadOnlyList<EpicGame> games, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine(BotMessages.EpicGamesHeader(lang));

        foreach (var game in games)
        {
            sb.AppendLine();
            sb.AppendLine($"<b>{EscapeHtml(game.Title)}</b>");

            if (!string.IsNullOrWhiteSpace(game.Description))
            {
                var description = game.Description.Length > 120
                    ? game.Description[..117] + "..."
                    : game.Description;
                sb.AppendLine(EscapeHtml(description));
            }

            sb.AppendLine($"🗓 {BotMessages.FreeUntilLabel(lang)}: {game.FreeUntil:dd.MM.yyyy}");
            sb.AppendLine($"🔗 <a href=\"{game.StoreUrl}\">{BotMessages.GetLabel(lang)}</a>");
        }

        return sb.ToString().TrimEnd();
    }

    private static string EscapeHtml(string text)
        => text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
