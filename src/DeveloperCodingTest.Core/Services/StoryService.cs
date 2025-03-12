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
        return SortEngineHelper.RankNStories(stories, topNStories > stories.Count 
            ? stories.Count 
            : topNStories);
    }
}
