using Dvizh.Application.Models.Input;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.CreateInvite;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class CreateInviteRequestMapper
{
    public static partial CreateInviteInput Map(CreateInviteRequest request);
}
