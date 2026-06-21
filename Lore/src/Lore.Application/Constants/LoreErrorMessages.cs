namespace Lore.Application.Constants;

public static class LoreErrorMessages
{
    public const string MovieNotFound = "Movie with id {0} was not found.";
    public const string UniverseNotFound = "Universe with id {0} was not found.";
    public const string ViewCountAlreadyZero = "Movie with id {0} has a view count of 0 and cannot be decremented.";
    public static string MovieAlreadyExists(string title, int releaseYear)
        => $"A movie with title '{title}' and release year {releaseYear} already exists.";
}
