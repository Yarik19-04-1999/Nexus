namespace Nexus.Application.Core.Interfaces;

public interface ISimpleUseCase<TInput>
{
    Task Execute(TInput input, CancellationToken cancellationToken);
}

public interface ISimpleUseCase<TInput, TOutput>
{
    Task<TOutput> Execute(TInput input, CancellationToken cancellationToken);
}