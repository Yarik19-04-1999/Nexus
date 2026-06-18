using Nexus.Core.Integration.Tests.Factories;

namespace Information.Integration.Tests.Infrastructure;

public class InformationDbFactory(InformationSqlFixture sqlFixture)
    : NexusSqlWebApplicationFactory<Program>(sqlFixture);
