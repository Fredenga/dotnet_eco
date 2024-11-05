using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedLib.Middleware;


namespace SharedLib.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, 
            IConfiguration config, 
            string fileName) where TContext: DbContext
        {
            // add generic DBContext
            services.AddDbContext<TContext>(options =>
            options.UseSqlServer(config.GetConnectionString("EcommerceConnection"),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            // configure serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add JWT authentication scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);

            
            return services;
        }

        public static IApplicationBuilder useSharedPolicies(this IApplicationBuilder app)
        {
            //use global exception
            app.UseMiddleware<GlobalException>();

            //register middleware to block all outside API calls
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
