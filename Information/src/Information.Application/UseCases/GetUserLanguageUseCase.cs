using Information.Application.Enums;
using Information.Application.Interfaces.Repositories;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetUserLanguageUseCase : IGetUserLanguageUseCase
{
    private readonly ITelegramUserRepository _userRepository;

    public GetUserLanguageUseCase(ITelegramUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<BotLanguage>> Execute(GetUserLanguageInput input, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(input.TelegramUserId, cancellationToken);

        var language = user?.Language ?? BotLanguage.Ukrainian;
        return Result<BotLanguage>.Success(language);
    }
}
