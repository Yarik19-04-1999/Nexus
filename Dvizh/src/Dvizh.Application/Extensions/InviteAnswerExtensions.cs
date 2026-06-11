using Dvizh.Application.Enums;

namespace Dvizh.Application.Extensions;

public static class InviteAnswerExtensions
{
    public static InviteEventType ToEventType(this InviteAnswer answer) => answer switch
    {
        InviteAnswer.Yes => InviteEventType.SaidYes,
        InviteAnswer.No => InviteEventType.SaidNo,
        _ => throw new ArgumentOutOfRangeException(nameof(answer), answer, null)
    };
}
