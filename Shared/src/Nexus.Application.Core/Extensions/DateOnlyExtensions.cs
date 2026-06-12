namespace Nexus.Application.Core.Extensions;

public static class DateOnlyExtensions
{
    public static DateOnly Yesterday(this DateOnly date) => date.AddDays(-1);

    public static DateOnly WeekAgo(this DateOnly date) => date.AddDays(-7);

    public static DateOnly MonthAgo(this DateOnly date) => date.AddMonths(-1);

    public static DateOnly YearAgo(this DateOnly date) => date.AddYears(-1);
}
