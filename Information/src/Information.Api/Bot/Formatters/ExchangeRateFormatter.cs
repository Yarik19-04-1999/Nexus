using System.Text;
using Information.Application.Enums;
using Information.Application.Models;
using Information.Api.Bot.Constants;
using Information.Api.Bot.Extensions;
using Information.Api.Bot.Localization;

namespace Information.Api.Bot.Formatters;

public static class ExchangeRateFormatter
{
    public static string FormatToday(IReadOnlyDictionary<ExchangeCurrency, ExchangeRate> rates, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.Append(BotMessages.RatesTodayHeader(lang));

        var date = rates.Values.FirstOrDefault()?.Date;
        if (date.HasValue)
        {
            sb.Append($" — {date.Value.ToString(IceAgeBriefTelegramBotConstants.DisplayDateFormat)}");
        }

        sb.AppendLine();
        sb.AppendLine();

        foreach (var (currency, rate) in rates)
        {
            sb.AppendLine($"{currency.ToFlag()} {currency}: {rate.Rate:F2} ₴");
        }

        return sb.ToString().TrimEnd();
    }

    public static string FormatHistory(IReadOnlyList<ExchangeRateHistory> histories, BotLanguage lang)
    {
        var sb = new StringBuilder();
        sb.AppendLine(BotMessages.RatesHistoryHeader(lang));

        foreach (var history in histories)
        {
            sb.AppendLine();
            sb.AppendLine($"{history.Currency.ToFlag()} {history.Currency}: {history.Current.Rate:F2} ₴");
            AppendHistoryRow(sb, BotMessages.YesterdayLabel(lang), history.Yesterday, history.Current);
            AppendHistoryRow(sb, BotMessages.WeekAgoLabel(lang), history.WeekAgo, history.Current);
            AppendHistoryRow(sb, BotMessages.MonthAgoLabel(lang), history.MonthAgo, history.Current);
            AppendHistoryRow(sb, BotMessages.YearAgoLabel(lang), history.YearAgo, history.Current);
        }

        return sb.ToString().TrimEnd();
    }

    private static void AppendHistoryRow(StringBuilder sb, string label, ExchangeRate? historical, ExchangeRate current)
    {
        if (historical is null)
        {
            return;
        }

        var change = (current.Rate - historical.Rate) / historical.Rate * 100m;
        var changeInt = (int)Math.Round(change);
        var arrow = changeInt >= 0 ? "🟢 ↑" : "🔴 ↓";
        var sign = changeInt >= 0 ? "+" : "";
        sb.AppendLine($"  {label}: {historical.Rate:F2} ₴  {arrow}{sign}{changeInt}%");
    }
}
