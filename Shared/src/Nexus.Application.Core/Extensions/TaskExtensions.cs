using Nexus.Application.Core.Constants;

namespace Nexus.Application.Core.Extensions;

public static class TaskExtensions
{
    public static async Task<T> ExecuteWithTimeout<T>(
        this Func<CancellationToken, Task<T>> operation,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);
        try
        {
            return await operation(cts.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException(TaskErrorMessages.TimedOut(timeout));
        }
    }
}
