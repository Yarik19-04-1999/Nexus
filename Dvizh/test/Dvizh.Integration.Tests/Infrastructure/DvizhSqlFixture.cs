using Nexus.Core.Integration.Tests.Fixtures;
using Xunit.Sdk;

[assembly: Xunit.AssemblyFixture(typeof(Dvizh.Integration.Tests.Infrastructure.DvizhSqlFixture))]

namespace Dvizh.Integration.Tests.Infrastructure;

public sealed class DvizhSqlFixture(IMessageSink messageSink) : NexusSqlFixture(messageSink);
