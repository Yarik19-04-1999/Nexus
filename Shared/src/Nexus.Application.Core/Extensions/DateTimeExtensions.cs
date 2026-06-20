namespace Nexus.Application.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToIsoString(this DateTime dateTime)
        => dateTime.ToString("o");
}
