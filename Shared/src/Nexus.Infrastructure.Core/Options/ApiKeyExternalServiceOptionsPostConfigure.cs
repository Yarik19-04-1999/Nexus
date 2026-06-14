using Microsoft.Extensions.Options;

namespace Nexus.Infrastructure.Core.Options;

public class ApiKeyExternalServiceOptionsPostConfigure : IPostConfigureOptions<ApiKeyExternalServiceOptions>
{
    public void PostConfigure(string? name, ApiKeyExternalServiceOptions options)
    {
        options.BaseUrl = options.BaseUrl.TrimEnd('/');
    }
}
