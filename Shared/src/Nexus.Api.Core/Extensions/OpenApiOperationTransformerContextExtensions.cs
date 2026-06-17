using Microsoft.AspNetCore.OpenApi;
using Nexus.Api.Core.ViewModels;

namespace Nexus.Api.Core.Extensions;

public static class OpenApiOperationTransformerContextExtensions
{
    public static async Task AddDefaultSchemas(this OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
    {
        await CreateDomainErrorSchema(context, cancellationToken);
        await CreateUnexpectodErrorSchema(context, cancellationToken);
    }

    public static async Task CreateDomainErrorSchema(this OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        => await CreateSchema<DomainErrorResponse>(context, cancellationToken);

    public static async Task CreateUnexpectodErrorSchema(this OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        => await CreateSchema<UnexpectedErrorResponse>(context, cancellationToken);

    public static async Task CreateSchema<T>(
        this OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken = default)
    {
        var schema = await context.GetOrCreateSchemaAsync(typeof(T), null, cancellationToken);
        context.Document?.AddComponent(typeof(T).Name, schema);
    }
}
