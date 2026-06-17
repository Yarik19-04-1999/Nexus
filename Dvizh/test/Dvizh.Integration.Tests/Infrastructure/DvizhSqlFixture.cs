using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Testcontainers.MsSql;
using Testcontainers.Xunit;
using Xunit;
using Xunit.Sdk;

[assembly: AssemblyFixture(typeof(Dvizh.Integration.Tests.Infrastructure.DvizhSqlFixture))]

namespace Dvizh.Integration.Tests.Infrastructure;

public sealed class DvizhSqlFixture : ContainerFixture<MsSqlBuilder, MsSqlContainer>
{
    public DvizhSqlFixture(IMessageSink messageSink) : base(messageSink)
    {
        
    }

    public string ConnectionString { get; private set; }

    protected override async ValueTask InitializeAsync()
    {
        await Container.StartAsync();
        await RunInitScripts();

        ConnectionString = new SqlConnectionStringBuilder(Container.GetConnectionString())
        {
            InitialCatalog = "Nexus"
        }.ConnectionString;
    }

    private async Task RunInitScripts()
    {
        await using var connection = new SqlConnection(Container.GetConnectionString());
        await connection.OpenAsync();

        foreach (var fileName in new[] { "create_database.sql", "script.sql" })
        {
            var path = Path.Combine(AppContext.BaseDirectory, "scripts", fileName);
            var sql = File.ReadAllText(path);
            var batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (var batch in batches)
            {
                var trimmed = batch.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                await using var cmd = new SqlCommand(trimmed, connection);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}

