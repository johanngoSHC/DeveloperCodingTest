namespace DeveloperCodingTest.Infrastructure.HttpClient;

using Core.Interfaces;
using System.Net.Http;
using System.Text.Json;

public class HackerNewsApiService : INewsApiService
{
    private readonly HttpClient _httpClient;

    public HackerNewsApiService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    public async Task<int[]> GetStoryIdsAsync()
    {
        var response = await _httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<int[]>(content) ?? [];
    }

    public async Task<string?> GetStoryDetailsAsync(int storyId)
    {
        var response = await _httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        return string.IsNullOrWhiteSpace(content) 
            ? string.Empty 
            : content;
    }
}