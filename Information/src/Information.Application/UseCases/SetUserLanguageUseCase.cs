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

        var user = await _userRepository.GetById(input.TelegramUserId, cancellationToken);

        var now = DateTime.UtcNow;
        user ??= new TelegramUser
        {
            TelegramUserId = input.TelegramUserId,
            CreatedAt = now
        };

        user.Language = input.Language;
        user.UpdatedAt = now;
        await _userRepository.Upsert(user, cancellationToken);
    }
}
