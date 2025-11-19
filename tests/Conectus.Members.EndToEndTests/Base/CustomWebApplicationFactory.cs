using Conectus.Members.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Conectus.Members.EndToEndTests.Base
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>, IDisposable
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var environment = "EndToEndTest";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            builder.UseEnvironment(environment);
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider
                .GetService<ConectusMemberDbContext>();
                ArgumentNullException.ThrowIfNull(context);
                context.Database.EnsureCreated();

                base.ConfigureWebHost(builder);
            });
        }
    }
}
