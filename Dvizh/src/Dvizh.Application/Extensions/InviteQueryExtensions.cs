using Dvizh.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Dvizh.Application.Extensions;

public static class InviteQueryExtensions
{
    public static Task<Invite?> FindById(this IQueryable<Invite> query, int id, CancellationToken ct = default)
        => query.FirstOrDefaultAsync(x => x.Id == id, ct);

    public static Task<Invite?> FindByCode(this IQueryable<Invite> query, string code, CancellationToken ct = default)
        => query.FirstOrDefaultAsync(x => x.Code == code, ct);

    public static Task<bool> CodeExists(this IQueryable<Invite> query, string code, CancellationToken ct = default)
        => query.AnyAsync(x => x.Code == code, ct);
}
