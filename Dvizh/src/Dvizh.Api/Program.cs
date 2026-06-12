using Dvizh.Application.Extensions;
using Nexus.Api.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddNexusApiVersioning();
builder.Services.AddApplication(builder.Configuration, builder.Environment);

var app = builder.Build();

app.MapControllers();
app.Run();
