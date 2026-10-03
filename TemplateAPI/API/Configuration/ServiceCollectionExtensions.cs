using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using Microsoft.OpenApi;

namespace API.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register repositories
        services.AddScoped<ISampleRepository, SampleRepository>();

        // Register services
        services.AddScoped<IExampleService, ExampleService>();

        // Add health checks
        services.AddHealthChecks();

        // Add endpoints API explorer (required for Swagger)
        services.AddEndpointsApiExplorer();

        // Add Swagger/OpenAPI
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Template API",
                Version = "v1"
            });
        });

        // Add controllers and authorization
        services.AddControllers();
        services.AddAuthorization();

        return services;
    }
}
