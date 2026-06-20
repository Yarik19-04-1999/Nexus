using Dvizh.Application.Constants;
using Dvizh.Application.DbContexts;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Options;
using Dvizh.Application.Services.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Services;
using Nexus.Infrastructure.Core.Extensions;
using Nexus.Infrastructure.EfCore.SqlServer.Extensions;
using Nexus.Infrastructure.Sieve.Extensions;

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
            .AddServices(configuration)
            .AddDvizhDbContext(configuration, environment);
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services
            .AddScoped<IGetInviteByIdUseCase, GetInviteByIdUseCase>()
            .AddScoped<IGetInvitesUseCase, GetInvitesUseCase>()
            .AddScoped<IGetInviteEventsUseCase, GetInviteEventsUseCase>()
            .AddScoped<ICreateInviteUseCase, CreateInviteUseCase>()
            .AddScoped<IUpdateInviteUseCase, UpdateInviteUseCase>()
            .AddScoped<IDeleteInviteUseCase, DeleteInviteUseCase>()
            .AddScoped<IResetInviteAnswerUseCase, ResetInviteAnswerUseCase>()
            .AddScoped<IOpenInviteUseCase, OpenInviteUseCase>()
            .AddScoped<IRespondToInviteUseCase, RespondToInviteUseCase>();

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSieve<DvizhSieveProcessor>(configuration, ConfigSectionConstants.Sieve)
            .AddOptions<InviteCodeGenerationOptions>()
                .BindConfiguration(nameof(InviteCodeGenerationOptions))
                .WithValidator<InviteCodeGenerationOptions, InviteCodeGenerationOptionsValidator>()
                .Services
            .AddSingleton<IInviteCodeGenerator, InviteCodeGenerator>()
            .AddSingleton<IUniqueCodeService, UniqueCodeService>();

    private static IServiceCollection AddDvizhDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var sqlServerOptions = configuration.GetSqlServerOptions();
        return services.AddNexusDbContext<DvizhDbContext>(sqlServerOptions, environment);
    }
}
