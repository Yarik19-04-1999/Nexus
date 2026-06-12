using Information.Application.Extensions;
using Information.Infrastructure.Extensions;
using Nexus.Api.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddNexusApiVersioning();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.Run();
