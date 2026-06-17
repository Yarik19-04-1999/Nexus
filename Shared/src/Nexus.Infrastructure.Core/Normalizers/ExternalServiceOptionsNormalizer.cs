using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Normalizers;

internal static class ExternalServiceOptionsNormalizer
{
    internal static void Normalize(ExternalServiceOptions options)
        => options.BaseUrl = options.BaseUrl.TrimEnd('/');
}
