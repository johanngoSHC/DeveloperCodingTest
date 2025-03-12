namespace DeveloperCodingTest.Api.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;

    public RateLimitMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse
            {
                StatusCode = StatusCodes.Status429TooManyRequests,
                Message = "You have exceeded the rate limit. Please try again later."
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
