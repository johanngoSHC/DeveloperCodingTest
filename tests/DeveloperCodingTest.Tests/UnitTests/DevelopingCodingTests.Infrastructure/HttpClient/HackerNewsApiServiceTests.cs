namespace DeveloperCodingTest.Tests.UnitTests.DevelopingCodingTests.Infrastructure.HttpClient;

using System.Net;
using System.Text;
using System.Text.Json;
using DeveloperCodingTest.Infrastructure.HttpClient;
using FakeItEasy;

public class HackerNewsApiServiceTests
{
    private System.Net.Http.HttpClient CreateFakeHttpClient(HttpResponseMessage response)
    {
        var handler = A.Fake<HttpMessageHandler>();
        A.CallTo(handler)
            .WithReturnType<Task<HttpResponseMessage>>()
            .Where(call => call.Method.Name == "SendAsync")
            .Returns(response);

        return new System.Net.Http.HttpClient(handler);
    }

    [Fact]
    public async Task GetStoryIdsAsync_ShouldReturnStoryIds_WhenResponseIsSuccessful()
    {
        // Arrange
        var expectedStoryIds = new[] { 12345, 67890, 54321 };
        var jsonResponse = JsonSerializer.Serialize(expectedStoryIds);
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        var httpClient = CreateFakeHttpClient(response);
        var hackerNewsService = new HackerNewsApiService(httpClient);

        // Act
        var result = await hackerNewsService.GetStoryIdsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStoryIds.Length, result.Length);
        Assert.Equal(expectedStoryIds, result);
    }

    [Fact]
    public async Task GetStoryIdsAsync_ShouldThrowException_WhenResponseIsNotSuccessful()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        var httpClient = CreateFakeHttpClient(response);
        var hackerNewsService = new HackerNewsApiService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => hackerNewsService.GetStoryIdsAsync());
    }
}