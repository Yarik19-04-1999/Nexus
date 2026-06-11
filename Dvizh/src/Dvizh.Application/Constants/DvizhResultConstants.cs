using Nexus.Application.Core.Models;

namespace Dvizh.Application.Constants;

public static class DvizhResultConstants
{
    public static Result AlreadyAnswered(int inviteId)
        => Result.Failure(DvizhErrorCodes.AlreadyAnswered, DvizhErrorMessages.AlreadyAnswered(inviteId));
}
