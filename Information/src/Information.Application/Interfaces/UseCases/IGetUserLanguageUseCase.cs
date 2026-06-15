using Information.Application.Enums;
using Information.Application.Models.Input;
using Nexus.Application.Core.Interfaces;

namespace Information.Application.Interfaces.UseCases;

public interface IGetUserLanguageUseCase : ISimpleUseCase<GetUserLanguageInput, BotLanguage>;
