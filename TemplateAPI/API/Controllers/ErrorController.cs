using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class ErrorController : ApiControllerBase
    {
        #region Methods

        [Route("/error")]
        [AcceptVerbs("GET", "POST", "PUT", "DELETE", "PATCH", "HEAD", "OPTIONS", "TRACE")]
        public IActionResult HandleError([FromServices] ILogger<ErrorController> logger, [FromServices] IHostEnvironment env)
        {
            logger.LogInformation("{ControllerName} => {ActionName} Started.", nameof(ErrorController), nameof(HandleError));

            IExceptionHandlerFeature? feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception?                ex      = feature?.Error;

            (int status, string title, string errorCode, string? detail) = MapToProblem(ex, env);

            if (ex is OperationCanceledException)
            {
                logger.LogInformation(ex, "Request canceled.");
            }
            else
            {
                logger.LogError(ex, "Unhandled exception: {Title}", title);
            }

            ProblemDetails problem = new()
            {
                Status   = status,
                Title    = title,
                Detail   = detail,
                Instance = HttpContext.Request.Path,
                Extensions =
                {
                    ["traceId"] = GetCorrelationId()
                }
            };

            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                problem.Extensions["errorCode"] = errorCode;
            }

            logger.LogInformation("{ControllerName} => {ActionName} Completed.", nameof(ErrorController), nameof(HandleError));

            return new ObjectResult(problem)
            {
                StatusCode = status
            };
        }

        #endregion


        #region Static Properties and Methods

        private static (int Status, string Title, string ErrorCode, string? Detail) MapToProblem(Exception? ex, IHostEnvironment env)
        {
            return ex switch
            {
                OperationCanceledException => 
                    (499, "Request canceled", "Common.RequestCanceled", "The request was canceled by the client."),

                KeyNotFoundException => 
                    (StatusCodes.Status404NotFound, "Resource not found", "Common.NotFound", "The requested resource does not exist."),

                HttpRequestException httpEx when httpEx.StatusCode.HasValue => 
                    ((int)httpEx.StatusCode.Value, "Downstream dependency error", "Common.DownstreamError", 
                    env.IsDevelopment() ? httpEx.Message : "A required downstream service returned an error."),

                _ => 
                    (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "Common.Unexpected", 
                    env.IsDevelopment() ? ex?.Message : "An unexpected error occurred. Try again later.")
            };
        }

        #endregion
    }
}
