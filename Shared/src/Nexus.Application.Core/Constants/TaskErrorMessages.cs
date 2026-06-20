namespace Nexus.Application.Core.Constants;

public static class TaskErrorMessages
{
    public static string TimedOut(TimeSpan timeout)
        => $"Operation timed out after {timeout.TotalSeconds}s.";
}
