using AuthApi.Application.Interfaces;
using AuthApi.Infrastructure.Data;
using AuthApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLib.DependencyInjection;

namespace AuthApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config) {
            // add db
            SharedServiceContainer.AddSharedServices<AuthDbContext>(services, config, config["MySerilog:FileName"]!);
            // add jwt auth scheme

            // create dependency injection 
            services.AddScoped<IUser, UserRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.useSharedPolicies(app);
            return app;
        }
    }
}
