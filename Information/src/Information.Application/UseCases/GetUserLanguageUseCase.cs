using Information.Application.Enums;
using Information.Application.Interfaces.Repositories;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;

namespace Information.Application.UseCases;

public class GetUserLanguageUseCase : IGetUserLanguageUseCase
{
    private readonly ITelegramUserRepository _userRepository;

    public GetUserLanguageUseCase(ITelegramUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BotLanguage> Execute(GetUserLanguageInput input, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(input.TelegramUserId, cancellationToken);
        return user?.Language ?? BotLanguage.Ukrainian;
    }
}
