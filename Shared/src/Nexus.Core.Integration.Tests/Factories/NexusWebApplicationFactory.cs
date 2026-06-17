using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Core.Integration.Tests.Factories;

public class NexusWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public T GetRequiredService<T>() where T : notnull =>
        Services.GetRequiredService<T>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
    }
}
