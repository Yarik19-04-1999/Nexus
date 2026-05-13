namespace Nexus.Application.Core.Interfaces;

public interface IUseCase<TInput, TOutput> where TOutput : IResult
{
    Task<TOutput> ExecuteAsync(TInput input, CancellationToken cancellationToken = default);
}
