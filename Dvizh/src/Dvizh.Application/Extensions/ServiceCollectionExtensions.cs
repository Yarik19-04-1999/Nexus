using Dvizh.Application.DbContexts;
using Dvizh.Application.Interfaces;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Services;
using Dvizh.Application.Services.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Infrastructure.Core.Extensions;
using Nexus.Infrastructure.EfCore.SqlServer.Extensions;

namespace Dvizh.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        return services
            .AddUseCases()
            .AddServices()
            .AddDvizhDbContext(configuration, environment);
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services
            .AddScoped<IGetInviteByIdUseCase, GetInviteByIdUseCase>()
            .AddScoped<ICreateInviteUseCase, CreateInviteUseCase>()
            .AddScoped<IUpdateInviteUseCase, UpdateInviteUseCase>()
            .AddScoped<IDeleteInviteUseCase, DeleteInviteUseCase>()
            .AddScoped<IResetInviteAnswerUseCase, ResetInviteAnswerUseCase>()
            .AddScoped<IOpenInviteUseCase, OpenInviteUseCase>()
            .AddScoped<IRespondToInviteUseCase, RespondToInviteUseCase>();

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddSingleton<IInviteCodeGenerator, InviteCodeGenerator>();

    private static IServiceCollection AddDvizhDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var sqlServerOptions = configuration.GetSqlServerOptions();
        return services.AddNexusDbContext<DvizhDbContext>(sqlServerOptions, environment);
    }
}
