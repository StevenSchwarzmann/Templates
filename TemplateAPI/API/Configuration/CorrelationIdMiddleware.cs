using Microsoft.Extensions.Primitives;

using Serilog.Context;

namespace API.Configuration;

/// <summary>
///     Middleware to manage correlation IDs for tracing requests across services.
///     Generates a new correlation ID if one is not provided in the request headers.
/// </summary>
public class CorrelationIdMiddleware
{
    #region Constants

    private const string CorrelationIdHeaderName = "X-Correlation-ID";

    #endregion


    #region Data Members

    private readonly RequestDelegate _next;

    #endregion


    #region Constructors

    public CorrelationIdMiddleware(RequestDelegate next) { _next = next ?? throw new ArgumentNullException(nameof(next)); }

    #endregion


    #region Methods

    public async Task InvokeAsync(HttpContext context)
    {
        // Try to get correlation ID from request headers
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId))
        {
            // Generate a new correlation ID if not provided
            correlationId = Guid.NewGuid()
                                .ToString("N");
        }

        string correlationIdValue = correlationId.ToString();

        // Store in HttpContext for easy access throughout the request
        context.Items["CorrelationId"] = correlationIdValue;

        // Add to response headers
        context.Response.Headers[CorrelationIdHeaderName] = correlationIdValue;

        using (LogContext.PushProperty("CorrelationId", correlationIdValue))
        {
            await _next(context);
        }
    }

    #endregion
}
