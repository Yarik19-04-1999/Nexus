using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Normalizers;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.PostConfigures;

public class ApiKeyExternalServiceOptionsPostConfigure : IPostConfigureOptions<ApiKeyExternalServiceOptions>
{
    public void PostConfigure(string? name, ApiKeyExternalServiceOptions options)
        => ExternalServiceOptionsNormalizer.Normalize(options);
}
