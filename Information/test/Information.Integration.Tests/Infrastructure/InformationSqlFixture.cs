using Nexus.Core.Integration.Tests.Fixtures;
using Xunit.Sdk;

[assembly: Xunit.AssemblyFixture(typeof(Information.Integration.Tests.Infrastructure.InformationSqlFixture))]

namespace Information.Integration.Tests.Infrastructure;

public sealed class InformationSqlFixture(IMessageSink messageSink) : NexusSqlFixture(messageSink);
