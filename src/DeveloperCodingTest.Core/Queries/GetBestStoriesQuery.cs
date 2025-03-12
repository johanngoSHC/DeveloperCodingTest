using DeveloperCodingTest.Core.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace DeveloperCodingTest.Core.Queries;

using Interfaces;
using MediatR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Entities.StoryAggregate;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class GetBestStoriesQuery : IRequest<List<StoryResponseDto>>
{
    public int N { get; }

    public GetBestStoriesQuery(int n)
    {
        N = n;
    }
}

public class GetBestStoriesQueryHandler : IRequestHandler<GetBestStoriesQuery, List<StoryResponseDto>>
{
    private readonly INewsApiService _hacNewsApiService;
    private readonly IStoryService _storyService;
    private readonly IMemoryCache _cache;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ILogger<GetBestStoriesQueryHandler> _logger;

    public GetBestStoriesQueryHandler(INewsApiService hacNewsApiService, IStoryService storyService, IMemoryCache cache, ILogger<GetBestStoriesQueryHandler> logger)
    {
        this._hacNewsApiService = hacNewsApiService;
        this._storyService = storyService;
        this._cache = cache; //TODO: Cache service can be reworked to use an instance of Redis
        this._jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        this._logger = logger; //TODO: Logger can be integrated with Serilog and be published to a cloud bucket
    }

    public async Task<List<StoryResponseDto>> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken)
    {
        this._logger.LogInformation("Handling GetBestStoriesQuery with N = {N}", request.N);
        var bestStories = await this._hacNewsApiService.GetStoryIdsAsync();
        var maxDegreeOfParallelism = Environment.ProcessorCount * 2;
        var storyEntries = new ConcurrentDictionary<int, Story>();

        await Parallel.ForEachAsync(
            bestStories,
            new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism, CancellationToken = cancellationToken },
            async (storyId, _) =>
            {
                var cacheKey = $"StoryDetails_{storyId}";
                if (!this._cache.TryGetValue(cacheKey, out string? storyDetailsJson))
                {
                    storyDetailsJson = await _hacNewsApiService.GetStoryDetailsAsync(storyId);
                    if (!string.IsNullOrWhiteSpace(storyDetailsJson))
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Set cache expiration time
                        this._cache.Set(cacheKey, storyDetailsJson, cacheEntryOptions);
                    }
                }

                if (!string.IsNullOrWhiteSpace(storyDetailsJson))
                {
                    var storyDto = JsonSerializer.Deserialize<StoryDto>(
                        storyDetailsJson,
                        _jsonSerializerOptions);

                    // Ensure mandatory params are present, such as Story URL and Story Score
                    if (!string.IsNullOrWhiteSpace(storyDto?.Url) && storyDto.Score > 0)
                    {
                        storyEntries.TryAdd(
                            storyId,
                            new Story(
                                id: storyId,
                                title: storyDto.Title,
                                uri: storyDto.Url,
                                postedBy: storyDto.By,
                                time: DateTimeExtensions.UnixTimestampToFormattedDateTime(storyDto.Time), // Convert Unix timestamp to ISO 8601 format
                                score: storyDto.Score,
                                commentCount: storyDto.Descendants)
                        );
                    }
                    else
                    {
                        this._logger.LogWarning("Missing mandatory params from story with Id = {id}", storyDto?.Id);
                    }
                }
            });

        // Create a dictionary of story IDs and their scores
        var storyScores = storyEntries.ToDictionary(pair => pair.Key, pair => pair.Value.Score);

        // Rank the stories and get the top N story IDs
        var rankedStoryIds = this._storyService.RankNStories(storyScores, request.N);

        // Create the result list of StoryDto
        var result = new List<StoryResponseDto>();

        foreach (var id in rankedStoryIds)
        {
            if (storyEntries.TryGetValue(id, out var story))
            {
                result.Add(new StoryResponseDto
                {
                    Title = story.Title,
                    Uri = story.Uri!,
                    PostedBy = story.PostedBy,
                    Time = story.Time.ToString("o"), // ISO 8601 format
                    Score = story.Score,
                    CommentCount = story.CommentCount
                });
            }
        }
        return result;
    }
}
