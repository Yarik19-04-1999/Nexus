using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Nexus.Infrastructure.EfCore.SqlServer.Extensions;

public static class DatabaseFacadeExtensions
{
    public static async Task<T> ExecuteInTransaction<T>(
        this DatabaseFacade database,
        Func<Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        var strategy = database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await database.BeginTransactionAsync(cancellationToken);
            var result = await operation();
            await transaction.CommitAsync(cancellationToken);
            return result;
        });
    }

    public static Task ExecuteInTransaction(
        this DatabaseFacade database,
        Func<Task> operation,
        CancellationToken cancellationToken = default)
    {
        var strategy = database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await database.BeginTransactionAsync(cancellationToken);
            await operation();
            await transaction.CommitAsync(cancellationToken);
        });
    }
}
