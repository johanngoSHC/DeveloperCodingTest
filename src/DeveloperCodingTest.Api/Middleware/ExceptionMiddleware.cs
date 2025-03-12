using DeveloperCodingTest.Core.Exceptions;
using System.Net;
using Polly.CircuitBreaker;

namespace DeveloperCodingTest.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case InvalidStoryException invalidStoryException:
                _logger.LogWarning(invalidStoryException, "Invalid story exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await context.Response.WriteAsync(new ErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = invalidStoryException.Message
                }.ToString());
                break;

            case BrokenCircuitException brokenCircuitException:
                _logger.LogWarning(brokenCircuitException, "Circuit breaker exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                await context.Response.WriteAsync(new ErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "The service is currently unavailable. Please try again later."
                }.ToString());
                break;

            case HttpRequestException httpRequestException:
                _logger.LogWarning(httpRequestException, "HTTP request exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await context.Response.WriteAsync(new ErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An error occurred while processing your request. Please try again later."
                }.ToString());
                break;

            default:
                _logger.LogError(exception, "An unhandled exception has occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new ErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Message
                }.ToString());
                break;
        }
    }
}
