namespace Nexus.Application.Core.Constants;

public static class CommonErrorMessages
{
    public static string NotFound<T>(int id)
        => $"{typeof(T).Name} with identifier {id} not found.";

    public static string NotFound<T>(string code)
        => $"{typeof(T).Name} with code '{code}' not found.";

    public static string AlreadyExpired<T>(int id)
        => $"{typeof(T).Name} with identifier {id} has already expired.";

    public static string CodeAlreadyExists<T>(string code)
        => $"{typeof(T).Name} with code '{code}' already exists.";
}
