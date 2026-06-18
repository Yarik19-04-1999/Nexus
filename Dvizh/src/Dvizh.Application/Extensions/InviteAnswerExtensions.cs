using Dvizh.Application.Enums;
using Dvizh.Application.Mappers;

namespace Dvizh.Application.Extensions;

public static class InviteAnswerExtensions
{
    public static InviteEventType ToEventType(this InviteAnswer answer) 
        => InviteAnswerMapper.ToEventType(answer);
}
