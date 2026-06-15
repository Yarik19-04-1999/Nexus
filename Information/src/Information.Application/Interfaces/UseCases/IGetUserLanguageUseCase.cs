using Information.Application.Enums;
using Information.Application.Models.Input;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.UseCases;

public interface IGetUserLanguageUseCase : IUseCase<GetUserLanguageInput, Result<BotLanguage>>;
