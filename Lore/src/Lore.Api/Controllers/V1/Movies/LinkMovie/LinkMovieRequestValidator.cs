using FluentValidation;

namespace Lore.Api.Controllers.V1.Movies.LinkMovie;

public class LinkMovieRequestValidator : AbstractValidator<LinkMovieRequest>
{
    public LinkMovieRequestValidator()
    {
        RuleFor(x => x.MovieId).GreaterThan(0);
        RuleFor(x => x.UniverseId).GreaterThan(0);
    }
}
