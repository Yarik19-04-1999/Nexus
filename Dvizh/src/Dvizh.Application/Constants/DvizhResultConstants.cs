using Dvizh.Application.Models;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Constants;

public static class DvizhResultConstants
{
    public static Result<Invite> InviteNotFound(string code)
        => Result<Invite>.Failure(CommonErrorCodes.NotFound, DvizhErrorMessages.InviteNotFound(code));

    public static Result CodeAlreadyExists(string code)
        => Result.Failure(DvizhErrorCodes.CodeAlreadyExists, DvizhErrorMessages.CodeAlreadyExists(code));

    public static Result AlreadyAnswered()
        => Result.Failure(DvizhErrorCodes.AlreadyAnswered, DvizhErrorMessages.AlreadyAnswered);
}
