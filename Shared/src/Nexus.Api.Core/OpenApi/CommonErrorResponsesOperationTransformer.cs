using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Nexus.Api.Core.ViewModels;
using System.Net.Mime;

namespace Nexus.Api.Core.OpenApi;

internal sealed class CommonErrorResponsesOperationTransformer : IOpenApiOperationTransformer
{
    private static readonly string Status418 = StatusCodes.Status418ImATeapot.ToString();
    private static readonly string Status500 = StatusCodes.Status500InternalServerError.ToString();

    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var domainErrorSchema = await context.GetOrCreateSchemaAsync(typeof(DomainErrorResponse), null, cancellationToken);
        context.Document?.AddComponent(nameof(DomainErrorResponse), domainErrorSchema);

        var unexpectedErrorSchema = await context.GetOrCreateSchemaAsync(typeof(UnexpectedErrorResponse), null, cancellationToken);
        context.Document?.AddComponent(nameof(UnexpectedErrorResponse), unexpectedErrorSchema);

        operation.Responses ??= new OpenApiResponses();

        if (!operation.Responses.ContainsKey(Status418))
        {
            operation.Responses[Status418] = new OpenApiResponse
            {
                Description = "Domain error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchemaReference(nameof(DomainErrorResponse), context.Document)
                    }
                }
            };
        }

        if (!operation.Responses.ContainsKey(Status500))
        {
            operation.Responses[Status500] = new OpenApiResponse
            {
                Description = "Unexpected server error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchemaReference(nameof(UnexpectedErrorResponse), context.Document)
                    }
                }
            };
        }
    }
}
