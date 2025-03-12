using DeveloperCodingTest.Core.Interfaces;
using DeveloperCodingTest.Infrastructure.HttpClient;
using Scalar.AspNetCore;
using System.Reflection;
using System.Threading.RateLimiting;
using DeveloperCodingTest.Core.Queries;
using DeveloperCodingTest.Core.Services;
using Microsoft.AspNetCore.RateLimiting;
using DeveloperCodingTest.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

//TODO: The solution can be reworked into minimal api for simplicity and remove bulky code
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetBestStoriesQuery)) ?? throw new InvalidOperationException()));
builder.Services.AddOpenApi();

// Register Core Services and its implementation
builder.Services.AddScoped<IStoryService, StoryService>();

// Register Memory Cache
builder.Services.AddMemoryCache();

// Define Polly policies
var retryPolicy = HackerNewsPolicy.GetRetryPolicy();
var circuitBreakerPolicy = HackerNewsPolicy.GetCircuitBreakerPolicy();

// Register Infrastructure Services
builder.Services.AddHttpClient<INewsApiService, HackerNewsApiService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(2))  // Improve performance and reliability by reusing available connections
    .AddPolicyHandler(retryPolicy) // Exponential Backoff to avoid overloading Hacker API
    .AddPolicyHandler(circuitBreakerPolicy); // Circuit Breaker to avoid overloading Hacker API 

// Configure rate limiting
//TODO: Rate limiting can be implemented directly on API Gateway at infrastructure level
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter(policyName: "GetBestStories", o =>
    {
        o.PermitLimit = 5;
        o.Window = TimeSpan.FromSeconds(60);
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Handle Exceptions
app.UseMiddleware<ExceptionMiddleware>();

// Use custom rate limit middleware
app.UseMiddleware<RateLimitMiddleware>();

// Use rate limiting middleware
app.UseRateLimiter();

//TODO: For some production API is not recommendable to expose this. For the purpose of this test we expose it 
if (app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("DeveloperCodingTest API")
            .WithDefaultHttpClient(ScalarTarget.Http, ScalarClient.Curl)
            .WithDarkModeToggle(true);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
