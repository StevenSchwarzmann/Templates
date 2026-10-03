using Serilog;

namespace API.Configuration;

public static class SerilogConfiguration
{
    #region Static Properties and Methods

    public static void ConfigureSerilog(IConfiguration configuration)
    {
        string instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? Environment.MachineName;

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
                                              .Enrich.FromLogContext()
                                              .Enrich.WithProperty("InstanceId", instanceId)
                                              .Enrich.WithProperty("CorrelationId", Guid.Empty)
                                              .Enrich.With(new DailySequenceNumberEnricher())
                                              .CreateLogger();
    }

    #endregion
}
