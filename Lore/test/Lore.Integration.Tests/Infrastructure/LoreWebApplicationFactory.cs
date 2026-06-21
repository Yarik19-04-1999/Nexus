using Nexus.Core.Integration.Tests.Factories;

namespace Lore.Integration.Tests.Infrastructure;

public class LoreWebApplicationFactory(LoreSqlFixture sqlFixture)
    : NexusSqlWebApplicationFactory<Program>(sqlFixture);
