using Dvizh.Application.Models.Input;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.UpdateInvite;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UpdateInviteRequestMapper
{
    public static partial UpdateInviteInput Map(UpdateInviteRequest request);
}
