using Information.Application.Interfaces.Repositories;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;

namespace Information.Application.UseCases;

public class SetUserLanguageUseCase : ISetUserLanguageUseCase
{
    private readonly ITelegramUserRepository _userRepository;

    public SetUserLanguageUseCase(ITelegramUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Execute(SetUserLanguageInput input, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetById(input.TelegramUserId, cancellationToken);

        if (existing is null)
        {
            var now = DateTime.UtcNow;
            var newUser = new TelegramUser
            {
                TelegramUserId = input.TelegramUserId,
                Language = input.Language,
                CreatedAt = now,
                UpdatedAt = now
            };
            await _userRepository.Upsert(newUser, cancellationToken);
        }
        else
        {
            existing.Language = input.Language;
            existing.UpdatedAt = DateTime.UtcNow;
            await _userRepository.Upsert(existing, cancellationToken);
        }
    }
}
