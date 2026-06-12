using Asp.Versioning;

namespace Nexus.Api.Core.Constants;

public static class ApiVersionConstants
{
    public static readonly ApiVersion V1 = new (1, 0);
    public static readonly ApiVersion V2 = new (2, 0);
    public static readonly ApiVersion V3 = new (3, 0);
    public static readonly ApiVersion V4 = new (4, 0);
    public static readonly ApiVersion V5 = new (5, 0);
}
