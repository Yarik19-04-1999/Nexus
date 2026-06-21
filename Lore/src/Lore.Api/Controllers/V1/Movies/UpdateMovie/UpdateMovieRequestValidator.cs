using FluentValidation;
using Lore.Application.Constants;

namespace Lore.Api.Controllers.V1.Movies.UpdateMovie;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(MovieValidationConstants.TitleMaxLength);
        RuleFor(x => x.ReleaseYear).GreaterThan(0);
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
        RuleFor(x => x.Score)
            .InclusiveBetween(MovieValidationConstants.ScoreMin, MovieValidationConstants.ScoreMax)
            .When(x => x.Score.HasValue);
        RuleFor(x => x.ViewCount).GreaterThan(0);
    }
}
