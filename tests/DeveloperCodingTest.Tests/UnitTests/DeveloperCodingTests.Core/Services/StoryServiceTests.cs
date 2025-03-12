namespace DeveloperCodingTest.Tests.UnitTests.DeveloperCodingTests.Core.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperCodingTest.Core.Services;
using TestHelpers;

public class StoryServiceTests
{
    private readonly StoryService _storyService = new();

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void RankNStories_ShouldReturnTopNStoriesOrderedByScoreDescending(int topN)
    {
        // Arrange
        var stories = TestDataGenerator.GenerateStories(500);

        // Act
        var result = _storyService.RankNStories(stories, topN);

        // Assert
        Assert.Equal(topN, result.Length);
        Assert.True(AreStoriesOrderedByScoreDescending(result, stories));
    }

    private bool AreStoriesOrderedByScoreDescending(int[] result, Dictionary<int, int> stories)
    {
        // Pair each story Id with the next story Id and check if the scores are in descending order
        return result.Zip(result.Skip(1), (a, b) => stories[a] >= stories[b]).All(x => x);
    }
}