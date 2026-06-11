namespace Dvizh.Application.Constants;

public static class DvizhErrorMessages
{
    public static string AlreadyAnswered(int inviteId)
        => $"Invite #{inviteId} has already been answered and cannot be changed.";
}
