namespace Lore.Application.Constants;

public static class LoreErrorMessages
{
    public static string MovieAlreadyExists(string title, int releaseYear)
        => $"A movie with title '{title}' and release year {releaseYear} already exists.";

    public static string UniverseAlreadyExists(string name)
        => $"A universe with name '{name}' already exists.";
}
