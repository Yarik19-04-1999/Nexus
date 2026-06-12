using Dvizh.Application.Models.Input;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.RespondToInvite;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class RespondToInviteRequestMapper
{
    public static partial RespondToInviteInput Map(RespondToInviteRequest request);
}
