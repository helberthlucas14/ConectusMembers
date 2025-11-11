using Conectus.Members.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Conectus.Members.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConections(
         this IServiceCollection services,
         IConfiguration configuration
        )
        {
            services.AddDbConnection(configuration);
            return services;
        }
        private static IServiceCollection AddDbConnection(
             this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration
                .GetConnectionString("ConectusDb");
            services.AddDbContext<ConectusMemberDbContext>(
                options => options.UseSqlServer(
                    connectionString
                )
            );
            return services;
        }
    }
}
