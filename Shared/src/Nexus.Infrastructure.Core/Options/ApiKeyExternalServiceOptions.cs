namespace Nexus.Infrastructure.Core.Options;

public class ApiKeyExternalServiceOptions : ExternalServiceOptions
{
    public string ApiKey { get; init; } = default!;
}
