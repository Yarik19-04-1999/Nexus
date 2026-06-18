using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class InviteMapper
{
    public static partial Invite MapCreate(CreateInviteInput input);

    public static partial void ApplyUpdate(UpdateInviteInput input, Invite invite);
}
