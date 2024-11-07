using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using SharedLib.DependencyInjection;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //Add database connectivity
            //Add Authentication scheme
            SharedServiceContainer.AddSharedServices < ProductDbContext>(services, config, config["MySerilog:FindName"]!);
            // Create dependency injection (Interface <-> Repo)
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //register middleware:
            //Global ex: external errors
            //ListenToApiGateway
            SharedServiceContainer.useSharedPolicies(app);
            return app;
        }
    }
}
