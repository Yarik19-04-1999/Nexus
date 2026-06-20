using Microsoft.EntityFrameworkCore;

namespace Nexus.Infrastructure.EfCore.SqlServer.Extensions;

public static class DbContextExtensions
{
    public static Task<T> ExecuteInTransaction<T>(
        this DbContext context,
        Func<Task<T>> operation,
        CancellationToken cancellationToken = default)
        => context.Database.ExecuteInTransaction(operation, cancellationToken);

    public static Task ExecuteInTransaction(
        this DbContext context,
        Func<Task> operation,
        CancellationToken cancellationToken = default)
        => context.Database.ExecuteInTransaction(operation, cancellationToken);
}
