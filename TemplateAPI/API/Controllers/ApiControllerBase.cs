using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    /// <summary>
    ///     Base controller providing common functionality for all API controllers,
    ///     including easy access to correlation IDs for request tracing.
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        #region Methods

        /// <summary>
        ///     Gets the correlation ID for this request.
        ///     Returns the ID from HttpContext.Items if available (set by middleware),
        ///     otherwise falls back to the TraceIdentifier.
        /// </summary>
        protected string GetCorrelationId()
        {
            HttpContext? httpContext = ControllerContext?.HttpContext ?? HttpContext;

            if (httpContext is null)
            {
                return Guid.NewGuid()
                           .ToString("N");
            }

            if (httpContext.Items.TryGetValue("CorrelationId", out object? correlationId))
            {
                return correlationId?.ToString()
                    ?? Guid.NewGuid()
                           .ToString("N");
            }

            return httpContext.TraceIdentifier
                ?? Guid.NewGuid()
                       .ToString("N");
        }

        #endregion
    }
}
