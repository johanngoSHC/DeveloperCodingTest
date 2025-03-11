namespace DeveloperCodingTest.Core.Services;

using System.Collections.Generic;
using Ardalis.GuardClauses;
using Interfaces;
using Helpers;

public class StoryService : IStoryService
{
    public int[] RankNStories(Dictionary<int, int> stories, int topNStories)
    {
        // Validate that topNStories is within the valid range
        Guard.Against.OutOfRange(topNStories, nameof(topNStories), 1, stories.Count, "The number of top stories must be between 1 and the total number of stories.");

        return SortEngineHelper.RankNStories(stories, topNStories);
    }
}
