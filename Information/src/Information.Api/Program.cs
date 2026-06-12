using Information.Application.Extensions;
using Information.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddNexusApiVersioning();
builder.Services.AddNexusOpenApi();
builder.Services.AddNexusCorrelationId();
builder.Services.AddNexusResponseCompression();
builder.Services.AddNexusRequestTimeouts();
builder.Services.AddNexusHealthChecks();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseNexusSecurityHeaders();
app.UseNexusCorrelationId();
app.UseNexusExceptionHandling();
app.UseNexusResponseCompression();
app.UseNexusRequestTimeouts();

app.MapControllers();
app.MapNexusHealthChecks();
app.MapNexusScalarUi();

app.Run();
