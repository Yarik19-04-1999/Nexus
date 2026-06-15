using Information.Infrastructure.DbContexts;
using Information.Application.Interfaces.Repositories;
using Information.Application.Models;

namespace Information.Infrastructure.Repositories;

public class TelegramUserRepository : ITelegramUserRepository
{
    private readonly InformationDbContext _context;

    public TelegramUserRepository(InformationDbContext context)
    {
        _context = context;
    }

    public async Task<TelegramUser?> GetById(long telegramUserId, CancellationToken cancellationToken = default)
    {
        return await _context.TelegramUsers.FindAsync([telegramUserId], cancellationToken);
    }

    public async Task Upsert(TelegramUser user, CancellationToken cancellationToken = default)
    {
        var existing = await _context.TelegramUsers.FindAsync([user.TelegramUserId], cancellationToken);

        if (existing is null)
        {
            _context.TelegramUsers.Add(user);
        }
        else
        {
            existing.Language = user.Language;
            existing.UpdatedAt = user.UpdatedAt;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
