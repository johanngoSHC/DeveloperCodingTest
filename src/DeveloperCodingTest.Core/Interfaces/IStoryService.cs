namespace DeveloperCodingTest.Core.Interfaces;

public interface IStoryService
{
    int[] RankNStories(Dictionary<int, int> stories, int topNStories);
}