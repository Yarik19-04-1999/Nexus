using FluentValidation;
using Lore.Application.Extensions;
using Lore.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.Options;
using Nexus.Infrastructure.Core.Constants;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddControllers();
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<Program>();
services
    .AddNexusServices(NexusOptions.Default)
    .AddNexusCors(configuration)
    .AddApplication()
    .AddInfrastructure(configuration, environment);

services
    .AddNexusHealthChecks()
    .AddNexusSqlServerHealthCheck(configuration[$"{OptionsConstants.SqlServer.SectionName}:ConnectionString"]!);

var app = builder.Build();

app.UseNexus(NexusOptions.Default)
   .UseCors();

app.MapControllers();
app.MapNexus(NexusOptions.Default);

app.Run();
