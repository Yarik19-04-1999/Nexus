using Information.Api.Bot.Extensions;
using Information.Application.Extensions;
using Information.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

var nexusOptions = configuration.GetNexusOptions();
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
