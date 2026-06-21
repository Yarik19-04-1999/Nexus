using FluentValidation;
using FluentValidation.AspNetCore;
using Lore.Application.Extensions;
using Lore.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;
using Nexus.Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

var sqlServerOptions = configuration.GetSqlServerOptions();
var nexusOptions = configuration.GetNexusOptions()
    .WithHealthCheckCustomAction(hc => hc.AddNexusSqlServerHealthCheck(sqlServerOptions.ConnectionString));

services.AddControllers();
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<Program>();
services
    .AddNexusServices(nexusOptions)
    .AddNexusCors(configuration)
    .AddApplication()
    .AddInfrastructure(configuration, environment);

var app = builder.Build();

app.UseNexus(nexusOptions).UseCors();
app.MapControllers();
app.MapNexus(nexusOptions);

app.Run();
