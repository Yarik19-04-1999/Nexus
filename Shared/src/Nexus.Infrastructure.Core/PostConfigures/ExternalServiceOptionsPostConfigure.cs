using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Normalizers;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.PostConfigures;

public class ExternalServiceOptionsPostConfigure : IPostConfigureOptions<ExternalServiceOptions>
{
    public void PostConfigure(string? name, ExternalServiceOptions options)
        => ExternalServiceOptionsNormalizer.Normalize(options);
}
