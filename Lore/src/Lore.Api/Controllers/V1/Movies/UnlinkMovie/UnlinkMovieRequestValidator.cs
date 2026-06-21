using FluentValidation;

namespace Lore.Api.Controllers.V1.Movies.UnlinkMovie;

public class UnlinkMovieRequestValidator : AbstractValidator<UnlinkMovieRequest>
{
    public UnlinkMovieRequestValidator()
    {
        RuleFor(x => x.MovieId).GreaterThan(0);
    }
}
