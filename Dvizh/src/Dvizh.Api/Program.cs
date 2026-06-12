using Dvizh.Application.Extensions;
using Nexus.Api.Core.Extensions;
using Nexus.Infrastructure.Core.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddNexusApiVersioning();
builder.Services.AddNexusOpenApi();
builder.Services.AddNexusCorrelationId();
builder.Services.AddNexusResponseCompression();
builder.Services.AddNexusRequestTimeouts();
builder.Services.AddNexusCors(builder.Configuration);
builder.Services.AddNexusHealthChecks()
    .AddNexusSqlServerHealthCheck(builder.Configuration[$"{OptionsConstants.SqlServer.SectionName}:ConnectionString"]!);
builder.Services.AddApplication(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseNexusSecurityHeaders();
app.UseNexusCorrelationId();
app.UseNexusExceptionHandling();
app.UseNexusResponseCompression();
app.UseNexusRequestTimeouts();
app.UseCors();
app.UseStaticFiles();

app.MapControllers();
app.MapNexusHealthChecks();
app.MapNexusScalarUi();

app.Run();
