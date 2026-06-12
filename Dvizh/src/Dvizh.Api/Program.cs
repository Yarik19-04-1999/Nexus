using Dvizh.Application.Extensions;
using Nexus.Api.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddNexusApiVersioning();
builder.Services.AddApplication(builder.Configuration, builder.Environment);
builder.Services.AddNexusCors(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseStaticFiles();
app.MapControllers();
app.Run();
