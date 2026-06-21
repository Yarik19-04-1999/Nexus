using Lore.Application.Models;
using Nexus.Application.Core.Models;

namespace Lore.Application.Constants;

public static class LoreResultConstants
{
    public static Result<Movie> MovieNotFound(int id) =>
        Result<Movie>.Failure(LoreErrorCodes.MovieNotFound, string.Format(LoreErrorMessages.MovieNotFound, id));

    public static Result UniverseNotFoundForMovie(int id) =>
        Result.Failure(LoreErrorCodes.UniverseNotFound, string.Format(LoreErrorMessages.UniverseNotFound, id));

    public static Result<Movie> ViewCountAlreadyZero(int id) =>
        Result<Movie>.Failure(LoreErrorCodes.ViewCountAlreadyZero, string.Format(LoreErrorMessages.ViewCountAlreadyZero, id));
}
