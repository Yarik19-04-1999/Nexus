using Information.Application.Models;

namespace Information.Application.Interfaces.Repositories;

public interface ITelegramUserRepository
{
    Task<TelegramUser?> GetById(long telegramUserId, CancellationToken cancellationToken = default);
    Task Upsert(TelegramUser user, CancellationToken cancellationToken = default);
}
