using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Interfaces.UseCases;

public interface IGetInviteEventsUseCase : IUseCase<GetInviteEventsInput, Result<PagedResult<InviteEvent>>>
{
}
