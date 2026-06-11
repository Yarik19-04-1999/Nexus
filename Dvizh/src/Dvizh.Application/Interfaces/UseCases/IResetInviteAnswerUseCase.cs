using Dvizh.Application.Models.Input;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Interfaces.UseCases;

public interface IResetInviteAnswerUseCase : IUseCase<ResetInviteAnswerInput, Result>
{
}
