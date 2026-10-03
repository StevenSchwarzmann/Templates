using API.Configuration;
using Azure.Identity;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<LoggerFilterOptions>(options =>
    {
        LoggerFilterRule? defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider");

        if (defaultRule is not null)
        {
            options.Rules.Remove(defaultRule);
        }

        options.MinLevel = LogLevel.Trace;
    });

    builder.Configuration.AddUserSecrets<Program>();
}

// Try to load Azure Key Vault configuration if URI is provided
string? keyVaultUri = builder.Configuration.GetValue<string>("AzureKeyVaultSettings:SecretsKeyVaultUri");

if (!string.IsNullOrEmpty(keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

// Configure Serilog
SerilogConfiguration.ConfigureSerilog(builder.Configuration);

// Add services to the container
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApiHealthChecks(builder.Configuration);

WebApplication app = builder.Build();

// Add correlation ID middleware early so downstream middleware and request logs share the same CorrelationId
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

// Map health check endpoints
app.MapApiHealthChecks();

// Enable Swagger in all environments (for template - consider restricting in production)
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template API v1");
    c.DocumentTitle = "Template API Docs";
    c.DocExpansion(DocExpansion.List);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Enable for testing
public partial class Program
{
    #region Constructors

    protected Program() { }

    #endregion
}
