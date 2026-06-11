using Dvizh.Application.Interfaces;
using Dvizh.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<IInviteCodeGenerator, InviteCodeGenerator>();

var app = builder.Build();

app.MapControllers();
app.Run();
