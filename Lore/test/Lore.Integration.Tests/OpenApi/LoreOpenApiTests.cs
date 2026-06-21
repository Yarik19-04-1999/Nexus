using Lore.Integration.Tests.Infrastructure;
using Nexus.Core.Integration.Tests.OpenApi;

namespace Lore.Integration.Tests.OpenApi;

public class LoreOpenApiTests(LoreWebApplicationFactory factory)
    : NexusOpenApiTests<Program, LoreWebApplicationFactory>(factory);
