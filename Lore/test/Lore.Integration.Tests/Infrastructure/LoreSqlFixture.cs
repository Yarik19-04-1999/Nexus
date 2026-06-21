using Nexus.Core.Integration.Tests.Fixtures;
using Xunit.Sdk;

[assembly: Xunit.AssemblyFixture(typeof(Lore.Integration.Tests.Infrastructure.LoreSqlFixture))]

namespace Lore.Integration.Tests.Infrastructure;

public sealed class LoreSqlFixture(IMessageSink messageSink) : NexusSqlFixture(messageSink);
