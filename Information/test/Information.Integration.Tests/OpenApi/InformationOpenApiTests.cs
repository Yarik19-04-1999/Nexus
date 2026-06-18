using Information.Integration.Tests.Infrastructure;
using Nexus.Api.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Integration.Tests.OpenApi;

namespace Information.Integration.Tests.OpenApi;

public class InformationOpenApiTests(InformationDbFactory factory)
    : NexusOpenApiTests(
        factory.CreateClient(),
        factory.WithConfiguration($"{ConfigSectionConstants.Configuration}:UseOpenApi", "false").CreateClient(),
        factory.GetOpenApiDocumentNames()),
      IClassFixture<InformationDbFactory>;
