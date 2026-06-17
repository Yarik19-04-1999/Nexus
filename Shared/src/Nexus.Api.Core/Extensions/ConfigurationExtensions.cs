using Microsoft.Extensions.Configuration;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.Options;
using Nexus.Application.Core.Extensions;

namespace Nexus.Api.Core.Extensions;

public static class ConfigurationExtensions
{
    public static NexusOptions GetNexusOptionsOrDefault(this IConfiguration configuration)
        => configuration.GetOptions<NexusOptions>(ConfigSectionConstants.Configuration) ?? new NexusOptions();
}
