namespace DeveloperCodingTest.Infrastructure.HttpClient;

using Polly;
using System;
using System.Net.Http;


public static class HackerNewsPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1.5, retryAttempt)));
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(msg => msg.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
            .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));
    }
}