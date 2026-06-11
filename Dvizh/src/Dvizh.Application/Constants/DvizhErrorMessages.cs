namespace Dvizh.Application.Constants;

public static class DvizhErrorMessages
{
    public static string InviteNotFound(string code)
        => $"Invite with code '{code}' was not found.";

    public static string CodeAlreadyExists(string code)
        => $"An invite with code '{code}' already exists.";

    public const string AlreadyAnswered = "This invite has already been answered and cannot be changed.";
}
