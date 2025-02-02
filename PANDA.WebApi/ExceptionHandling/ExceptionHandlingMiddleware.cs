using PANDA.Service.Exceptions;

namespace PANDA.WebApi.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                HttpStatusCode = StatusCodes.Status500InternalServerError,
                ErrorDetails = "An unexpected error occurred."
            };

            switch (exception)
            {
                case HandledException customException:
                    response.StatusCode = customException.StatusCode;
                    errorResponse.HttpStatusCode = customException.StatusCode;
                    errorResponse.ErrorDetails = customException.Message;
                    break;
                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.HttpStatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.IsUnhandledException = true;
                    errorResponse.ExceptionMessage = exception.Message;
                    break;
            }

            return response.WriteAsJsonAsync(errorResponse);
        }
    }

}
