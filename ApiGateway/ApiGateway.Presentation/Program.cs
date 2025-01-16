using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using SharedLib.DependencyInjection;
using ApiGateway.Presentation.Middleware;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());

JWTAuthenticationScheme.AddJWTAuthenticationScheme(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
}
);

// Add services to the container.

var app = builder.Build();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AttachSignatureToRequest>();
app.UseOcelot().Wait();

// Configure the HTTP request pipeline.
app.Run();






