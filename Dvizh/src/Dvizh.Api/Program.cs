using Dvizh.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication(builder.Configuration, builder.Environment);

var app = builder.Build();

app.MapControllers();
app.Run();
