using Information.Integration.Tests.Infrastructure;
using Nexus.Core.Integration.Tests.OpenApi;

namespace Information.Integration.Tests.OpenApi;

public class InformationOpenApiTests(InformationDbFactory factory)
    : NexusOpenApiTests<Program, InformationDbFactory>(factory);
