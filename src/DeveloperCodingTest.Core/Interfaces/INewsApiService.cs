namespace DeveloperCodingTest.Core.Interfaces;

using Entities.StoryAggregate;

public interface INewsApiService
{
    Task<int[]> GetStoryIdsAsync();
    Task<string?> GetStoryDetailsAsync(int storyIds);
}
