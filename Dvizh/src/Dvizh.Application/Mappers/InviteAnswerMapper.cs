using Dvizh.Application.Enums;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Application.Mappers;

[Mapper(RequiredEnumMappingStrategy = RequiredMappingStrategy.Source)]

public static partial class InviteAnswerMapper
{
    [MapEnumValue(InviteAnswer.Yes, InviteEventType.SaidYes)]
    [MapEnumValue(InviteAnswer.No, InviteEventType.SaidNo)]
    public static partial InviteEventType ToEventType(InviteAnswer answer);
}
