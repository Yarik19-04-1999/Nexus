namespace Nexus.Infrastructure.Core.Options;

internal static class ExternalServiceOptionsNormalizer
{
    internal static void Normalize(ExternalServiceOptions options)
        => options.BaseUrl = options.BaseUrl.TrimEnd('/');
}
