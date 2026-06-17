namespace Nexus.Application.Core.Utils;

public static class StringUtils
{
    private const string DefaultEllipsis = "...";

    public static string TruncateWithEllipsis(string value, int maxLength, string ellipsis = DefaultEllipsis)
    {
        if (ellipsis.Length >= maxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(ellipsis),
                $"Ellipsis length ({ellipsis.Length}) must be less than maxLength ({maxLength}).");
        }

        if (value.Length <= maxLength)
        {
            return value;
        }

        return value[..(maxLength - ellipsis.Length)] + ellipsis;
    }
}
