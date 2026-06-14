using Microsoft.Extensions.Options;

namespace Nexus.Infrastructure.Core.Options;

public class ExternalServiceOptionsPostConfigure : IPostConfigureOptions<ExternalServiceOptions>
{
    public void PostConfigure(string? name, ExternalServiceOptions options)
        => ExternalServiceOptionsNormalizer.Normalize(options);
}
