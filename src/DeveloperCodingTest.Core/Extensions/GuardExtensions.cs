namespace DeveloperCodingTest.Core.Extensions;

using Ardalis.GuardClauses;
using Exceptions;
using System.Globalization;


public static class GuardExtensions
{
    public static void InvalidStoryId(this IGuardClause guardClause, int id)
    {
        if (id <= 0)
        {
            throw new InvalidStoryException($"Story {nameof(id)} must be greater than zero.");
        }
    }

    public static void InvalidStoryScore(this IGuardClause guardClause, int score)
    {
        if (score <= 0)
        {
            throw new InvalidStoryException($"Story {nameof(score)} must be greater than zero.");
        }
    }

    public static void InvalidStoryUri(this IGuardClause guardClause, string? uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            throw new InvalidStoryException($"Story {nameof(uri)} must not be null or empty");
        }
    }

    public static void InvalidStoryDateTime(this IGuardClause guardClause, string? time)
    {
        if (string.IsNullOrWhiteSpace(time) || !DateTime.TryParseExact(
                time,
                "yyyy-MM-ddTHH:mm:ssK",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
        {
            throw new InvalidStoryException($"Story {nameof(time)} must be in the format 'yyyy-MM-ddTHH:mm:ssK'");
        }
    }
}