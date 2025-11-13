using Conectus.Members.Application.Interfaces;
using Conectus.Members.Application.UseCases.Member.CreateMember;
using Conectus.Members.Domain.Repository;
using Conectus.Members.Infra.Data.EF;
using Conectus.Members.Infra.Data.EF.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace Conectus.Members.Api.Configurations
{
    public static class UseCasesConfiguration
    {
        public static IServiceCollection AddUseCases(
    this IServiceCollection services
)
        {
            services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
               );
            services.AddRepositories();
            return services;
        }

        private static IServiceCollection AddRepositories(
                this IServiceCollection services
            )
        {
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
