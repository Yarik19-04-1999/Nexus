using Nexus.Core.Integration.Tests.Factories;

namespace Dvizh.Integration.Tests.Infrastructure;

public class DvizhWebApplicationFactory(DvizhSqlFixture sqlFixture)
    : NexusSqlWebApplicationFactory<Program>(sqlFixture);
