namespace DeveloperCodingTest.Core.Queries;

public class StoryResponseDto
{
    public string? Title { get; set; }
    public required string Uri { get; set; }
    public string? PostedBy { get; set; }
    public string? Time { get; set; }
    public required int Score { get; set; }
    public int CommentCount { get; set; }
}