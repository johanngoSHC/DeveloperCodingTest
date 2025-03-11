namespace DeveloperCodingTest.Tests.UnitTests.DeveloperCodingTests.Core.Helper;

using System.Collections.Generic;
using TestHelpers;
using Xunit;
using DeveloperCodingTest.Core.Helpers;

public class SortEngineHelperTests
{
    private const int Count = 500;

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(500)]
    public void RankNStories_ShouldReturnTopNStoriesOrderedByScoreDescending(int topN)
    {
        // Arrange
        var stories = TestDataGenerator.GenerateStories(Count);

        // Act
        var result = SortEngineHelper.RankNStories(stories, topN);

        // Assert
        Assert.Equal(topN, result.Length);
        Assert.True(AreStoriesOrderedByScoreDescending(result, stories));
    }

    private static bool AreStoriesOrderedByScoreDescending(int[] result, Dictionary<int, int> stories)
    {
        // Pair each story ID with the next story ID and check if the scores are in descending order
        return result.Zip(result.Skip(1), (a, b) => stories[a] >= stories[b]).All(x => x);
    }
}