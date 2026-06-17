using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.ViewModels;
using System.Net.Mime;

namespace Nexus.Api.Core.OpenApi;

public sealed class CommonErrorResponsesOperationTransformer : IOpenApiOperationTransformer
{
    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        await context.AddDefaultSchemas(cancellationToken);

        operation.Responses ??= [];
        operation.Responses.TryAddDefaultResponses(context.Document);
    }
}
