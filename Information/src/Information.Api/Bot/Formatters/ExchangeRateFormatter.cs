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
        sb.AppendLine();

        foreach (var history in histories)
        {
            sb.Append($"{history.Currency.ToFlag()} {history.Currency}: {history.Current.Rate:F2} ₴");

            if (history.YearAgo is not null)
            {
                var change = (history.Current.Rate - history.YearAgo.Rate) / history.YearAgo.Rate * 100m;
                var changeInt = (int)Math.Round(change);
                var arrow = changeInt >= 0 ? "🟢 ↑" : "🔴 ↓";
                var sign = changeInt >= 0 ? "+" : "";
                sb.Append($"  {arrow}{sign}{changeInt}%");
            }

            sb.AppendLine();
        }

        if (histories.Count == 1 && histories[0].YearAgo is not null)
        {
            sb.AppendLine();
            sb.AppendLine($"{BotMessages.TodayLabel(lang)}: {histories[0].Current.Rate:F2} ₴  ({histories[0].Current.Date.ToString(IceAgeBriefTelegramBotConstants.DisplayDateFormat)})");
            sb.AppendLine($"{BotMessages.YearAgoLabel(lang)}: {histories[0].YearAgo!.Rate:F2} ₴  ({histories[0].YearAgo!.Date.ToString(IceAgeBriefTelegramBotConstants.DisplayDateFormat)})");
        }

        return sb.ToString().TrimEnd();
    }
}
