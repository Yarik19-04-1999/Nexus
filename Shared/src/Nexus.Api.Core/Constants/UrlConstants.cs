namespace Nexus.Api.Core.Constants;

public static class UrlConstants
{
    public static class Health
    {
        public const string Suffix = "/health";
        public const string Live = $"{Suffix}/live";
        public const string Ready = $"{Suffix}/ready";
    }
}
