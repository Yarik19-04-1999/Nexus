using Information.Api.Bot.Extensions;
using Information.Application.Extensions;
using Information.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;
using Nexus.Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

var sqlServerOptions = configuration.GetSqlServerOptions();
var nexusOptions = configuration.GetNexusOptions()
    .WithHealthCheckCustomAction(hc => hc.AddNexusSqlServerHealthCheck(sqlServerOptions.ConnectionString));

services
    .AddNexusServices(nexusOptions)
    .AddApplication()
    .AddInfrastructure(configuration, builder.Environment)
    .AddIceAgeBriefTelegramBot()
    .AddControllers();

var app = builder.Build();
app.UseNexus(nexusOptions);
app.MapControllers();
app.MapNexus(nexusOptions);

app.Run();

public partial class Program { }
