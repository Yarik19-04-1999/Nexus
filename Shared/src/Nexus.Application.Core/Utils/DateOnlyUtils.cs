namespace Nexus.Application.Core.Utils;

public static class DateOnlyUtils
{
    public static DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.UtcNow);
}
