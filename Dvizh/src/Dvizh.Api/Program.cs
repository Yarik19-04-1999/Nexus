using Dvizh.Application.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
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
    .AddApplication(configuration, environment);

var app = builder.Build();

app.UseNexus(nexusOptions)
   .UseCors()
   .UseStaticFiles();

app.MapControllers();
app.MapNexus(nexusOptions);

app.Run();

public partial class Program { }
