namespace DeveloperCodingTest.Tests.UnitTests.DeveloperCodingTests.Core.Entities;

using DeveloperCodingTest.Core.Entities.StoryAggregate;
using DeveloperCodingTest.Core.Exceptions;

public class StoryTests
{
    private const int StoryId = 45789;
    private const string StoryTitle = "This is a test story Title";
    private const string StoryPostedBy = "afakeuser001";
    private const int StoryCommentCount = 258;
    private const string StoryUri = "http://fakeuri.com";
    private const string StoryDateTime = "2019-10-12T13:43:01+00:00";
    private const int StoryScore = 2345;

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_ThrowException_When_IdIsInvalid(int invalidId)
    {
        // Arrange
        var id = invalidId;

        // Act & Assert
        var exception = Assert.Throws<InvalidStoryException>(() => new Story(id, StoryTitle, StoryUri, StoryPostedBy, StoryDateTime, StoryScore, StoryCommentCount));
        Assert.True(exception.Message == $"Story {nameof(id)} must be greater than zero.",
            $"Expected exception message: 'Story {nameof(id)} must be greater than zero.', but got: '{exception.Message}'");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_ThrowException_When_UriIsInvalid(string? invalidUri)
    {
        // Arrange
        var uri = invalidUri;

        // Act & Assert
        var exception = Assert.Throws<InvalidStoryException>(() => new Story(StoryId, StoryTitle, uri, StoryPostedBy, StoryDateTime, StoryScore, StoryCommentCount));
        Assert.True(exception.Message == $"Story {nameof(uri)} must not be null or empty",
            $"Expected exception message: 'Story {nameof(uri)} must not be null or empty', but got: '{exception.Message}'");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_ThrowException_When_ScoreIsInvalid(int invalidScore)
    {
        // Arrange
        var score = invalidScore;

        // Act & Assert
        var exception = Assert.Throws<InvalidStoryException>(() => new Story(StoryId, StoryTitle, StoryUri, StoryPostedBy, StoryDateTime, score, StoryCommentCount));
        Assert.True(exception.Message == $"Story {nameof(score)} must be greater than zero.",
            $"Expected exception message: 'Story {nameof(score)} must be greater than zero.', but got: '{exception.Message}'");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("2019-10-12")]
    [InlineData("2019-10-12 13:43:01+00:00")]
    [InlineData("2019-13-12T13:43:01+00:00")]
    public void Should_ThrowException_When_DateTimeIsInvalid(string? invalidTime)
    {
        // Arrange
        var time = invalidTime;

        // Act & Assert
        var exception = Assert.Throws<InvalidStoryException>(() => new Story(StoryId, StoryTitle, StoryUri, StoryPostedBy, time, StoryScore, StoryCommentCount));
        Assert.True(exception.Message == $"Story {nameof(time)} must be in the format 'yyyy-MM-ddTHH:mm:ssK'",
            $"Expected exception message: 'Story {nameof(time)} must be in the format 'yyyy-MM-ddTHH:mm:ssK', but got: '{exception.Message}'");
    }

    [Fact]
    public void Should_CreateStory_When_AllParametersAreValid()
    {
        // Arrange
        var id = StoryId;
        var title = StoryTitle;
        var uri = StoryUri;
        var postedBy = StoryPostedBy;
        var dateTime = StoryDateTime;
        var score = StoryScore;
        var commentCount = StoryCommentCount;

        // Act
        var story = new Story(id, title, uri, postedBy, dateTime, score, commentCount);

        // Assert
        Assert.NotNull(story);
        Assert.Equal(id, story.Id);
        Assert.Equal(title, story.Title);
        Assert.Equal(uri, story.Uri);
        Assert.Equal(postedBy, story.PostedBy);
        Assert.Equal(DateTime.Parse(dateTime), story.Time);
        Assert.Equal(score, story.Score);
        Assert.Equal(commentCount, story.CommentCount);
    }

    [Fact]
    public void GetIdAndScore_ShouldReturnIdAndScoreAsKeyValuePair()
    {
        // Arrange
        var story = new Story(StoryId, StoryTitle, StoryUri, StoryPostedBy, StoryDateTime, StoryScore, StoryCommentCount);

        // Act
        var idAndScore = story.GetIdAndScore();

        // Assert
        Assert.Equal(StoryId, idAndScore.Key);
        Assert.Equal(StoryScore, idAndScore.Value);
        Assert.IsType<KeyValuePair<int, int>>(idAndScore);
    }
}
