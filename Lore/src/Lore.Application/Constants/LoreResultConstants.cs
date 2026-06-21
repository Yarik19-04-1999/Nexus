using Lore.Application.Models;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.Constants;

public static class LoreResultConstants
{
    public static Result<Movie> MovieNotFound(int id)
        => Result<Movie>.Failure(CommonErrorCodes.NotFound, CommonErrorMessages.NotFound<Movie>(id));

    public static Result<Movie> ViewCountAlreadyZero(int id)
        => Result<Movie>.Failure(LoreErrorCodes.ViewCountAlreadyZero, LoreErrorMessages.ViewCountAlreadyZero(id));

    public static Result MovieAlreadyExists(string title, int releaseYear)
        => Result.Failure(LoreErrorCodes.AlreadyExists, LoreErrorMessages.MovieAlreadyExists(title, releaseYear));

    public static Result UniverseAlreadyExists(string name)
        => Result.Failure(LoreErrorCodes.AlreadyExists, LoreErrorMessages.UniverseAlreadyExists(name));
}
