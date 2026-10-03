using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.Configuration;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecks = services.AddHealthChecks();

        // Add basic health check
        healthChecks.AddCheck("self", () => HealthCheckResult.Healthy("Template API is healthy"));

        // TODO: Add database health checks when you implement actual database connections
        // Example:
        // var connectionString = configuration.GetConnectionString("DefaultConnection");
        // if (!string.IsNullOrEmpty(connectionString))
        // {
        //     healthChecks.AddSqlServer(connectionString, name: "Default Database");
        // }

        return services;
    }

    public static WebApplication MapApiHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    timestamp = DateTime.UtcNow,
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description,
                        duration = e.Value.Duration.TotalMilliseconds
                    })
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        });

        app.MapHealthChecks("/health/ready");
        app.MapHealthChecks("/health/live");

        return app;
    }
}
