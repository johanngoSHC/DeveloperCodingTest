using System.Globalization;

namespace DeveloperCodingTest.Core.Entities.StoryAggregate;

using Ardalis.GuardClauses;
using Extensions;
using Interfaces;


public class Story : IAggregateRoot
{
    public int Id { get; private set; }
    public string? Title { get; private set; }
    public string? Uri { get; private set; }
    public string? PostedBy { get; private set; }
    public DateTime Time { get; private set; }
    public int Score { get; private set; }
    public int CommentCount { get; private set; }

    public Story(int id, string? title, string? uri, string postedBy, string? time, int score, int commentCount)
    {
        Guard.Against.InvalidStoryId(id);
        Guard.Against.InvalidStoryUri(uri);
        Guard.Against.InvalidStoryScore(score);
        Guard.Against.InvalidStoryDateTime(time);

        this.Id = id;
        this.Title = title;
        this.Uri = uri;
        this.PostedBy = postedBy;
        this.Time = DateTime.ParseExact(
            time!,
            "yyyy-MM-ddTHH:mm:ssK",
            CultureInfo.InvariantCulture);
        this.Score = score;
        this.CommentCount = commentCount;
    }
}