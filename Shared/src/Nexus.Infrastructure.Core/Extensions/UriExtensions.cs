namespace Nexus.Infrastructure.Core.Extensions;

public static class UriExtensions
{
    public static bool IsHttpOrHttps(this Uri uri)
    {
        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }
}
