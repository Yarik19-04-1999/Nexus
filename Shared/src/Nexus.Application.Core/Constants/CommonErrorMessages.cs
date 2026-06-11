namespace Nexus.Application.Core.Constants;

public static class CommonErrorMessages
{
    public static string NotFound<T>(int id)
        => NotFound(typeof(T).Name, id);

    public static string NotFound(string entityName, int id)
        => $"{entityName} with identifier {id} not found.";

    public const string AlreadyExpired = "This resource has already expired.";
}
