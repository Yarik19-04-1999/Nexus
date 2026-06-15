using Information.Application.Extensions;
using Information.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.Options;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services
    .AddNexusServices(NexusOptions.Default)
    .AddApplication()
    .AddInfrastructure(configuration);

var app = builder.Build();

app.UseNexus(NexusOptions.Default);

app.MapControllers();
app.MapNexus(NexusOptions.Default);

app.Run();
