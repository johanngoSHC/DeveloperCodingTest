namespace DeveloperCodingTest.Core.Helpers;

//TODO: Current implementation is enough for sorting small data collections. For higher inputs would recommend a merge sort implementation
public static class SortEngineHelper
{
    /// <summary>
    /// Ranks stories by their scores in descending order.
    /// </summary>
    /// <param name="stories">A dictionary where the key is the story Id and the value is the story Score.</param>
    /// <returns>An array of integers representing story Id's ordered by their scores from highest to lowest.</returns>
    private static int[] RankStories(Dictionary<int, int> stories)
    {
        return stories
            .OrderByDescending(s => s.Value) // Order by score descending
            .Select(s => s.Key) // Select only the story IDs (keys)
            .ToArray(); // Convert to array
    }

    /// <summary>
    /// Returns the top N stories ranked by their scores in descending order.
    /// </summary>
    /// <param name="stories">A dictionary where the key is the story Id and the value is the story Score.</param>
    /// <param name="topNStories">The number of top stories to return.</param>
    /// <returns>An array of integers representing the top N story Id's ordered by their scores from highest to lowest.</returns>
    public static int[] RankNStories(Dictionary<int, int> stories, int topNStories)
    {
        return RankStories(stories) // Rank all stories
            .Take(topNStories) // Take the top N stories
            .ToArray(); // Convert to array
    }
}
