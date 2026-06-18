using System.Text.Json;

namespace Nexus.Core.Integration.Tests.Constants;

public static class JsonSerializerConstants
{
    public static readonly JsonSerializerOptions HealthCheck = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
}
