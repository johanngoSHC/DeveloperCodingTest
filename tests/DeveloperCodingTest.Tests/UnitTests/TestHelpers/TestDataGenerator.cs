namespace DeveloperCodingTest.Tests.UnitTests.TestHelpers;

using System;
using System.Collections.Generic;

public static class TestDataGenerator
{
    public static Dictionary<int, int> GenerateStories(int count)
    {
        var random = new Random();
        var stories = new Dictionary<int, int>();

        for (var i = 0; i < count; i++)
        {
            int storyId;
            do
            {
                storyId = random.Next(10000000, 100000000); // Generate 8-digit story ID
            } while (stories.ContainsKey(storyId)); // Ensure unique story ID

            var storyScore = random.Next(1000, 10000); // Generate 4-digit story score
            stories.Add(storyId, storyScore);
        }

        return stories;
    }
}