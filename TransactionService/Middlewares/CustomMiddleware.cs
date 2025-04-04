namespace TransactionService.Middlewares;

public class CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception");

            context.Response.ContentType = "application/problem+json";

            var (statusCode, title) = exception switch
            {
                ArgumentException => (HttpStatusCode.BadRequest, "Validation error"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Not found"),
                OperationCanceledException => (HttpStatusCode.RequestTimeout, "Request was cancelled"),
                _ => (HttpStatusCode.InternalServerError, "Unexpected error")
            };

            context.Response.StatusCode = (int)statusCode;

            var problem = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{(int)statusCode}",
                Title = title,
                Detail = exception.Message,
                Status = (int)statusCode,
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
