using Lore.Application.Models;
using Nexus.Application.Core.Models;
using Sieve.Models;

namespace Lore.Application.Interfaces.Stores;

public interface ILoreStore
{
    Task<PagedResult<Universe>> GetUniversesPaged(SieveModel sieve, CancellationToken cancellationToken);
    Task<Universe?> GetUniverseById(int id, CancellationToken cancellationToken);
    Task CreateUniverse(Universe universe, CancellationToken cancellationToken);
    Task UpdateUniverse(Universe universe, CancellationToken cancellationToken);
    Task DeleteUniverse(Universe universe, CancellationToken cancellationToken);
    Task<IReadOnlyList<Universe>> SearchUniverses(string query, CancellationToken cancellationToken);
    Task<PagedResult<Movie>> GetMoviesPaged(SieveModel sieve, CancellationToken cancellationToken);
    Task<Movie?> GetMovieById(int id, CancellationToken cancellationToken);
    Task CreateMovie(Movie movie, CancellationToken cancellationToken);
    Task UpdateMovie(Movie movie, CancellationToken cancellationToken);
    Task DeleteMovie(Movie movie, CancellationToken cancellationToken);
    Task<IReadOnlyList<Movie>> SearchMovies(string query, CancellationToken cancellationToken);
}
