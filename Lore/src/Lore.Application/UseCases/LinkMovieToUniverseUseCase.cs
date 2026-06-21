using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class LinkMovieToUniverseUseCase : ILinkMovieToUniverseUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public LinkMovieToUniverseUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result> Execute(LinkMovieToUniverseInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.MovieId, cancellationToken);
        var validationResult = await _validators.CreateLinkMovieToUniverseValidator(new LinkMovieToUniverseValidationContext(movie))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult();
        }

        LinkMovieToUniverseMapper.Map(input, movie!);
        movie!.UpdatedAt = DateTime.UtcNow;
        await _store.UpdateMovie(movie, cancellationToken);
        return Result.Success();
    }
}
