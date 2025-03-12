namespace DeveloperCodingTest.Core.Extensions;

public static class DateTimeExtensions
{
    public static string UnixTimestampToFormattedDateTime(long unixTimestamp)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        var dateTime = dateTimeOffset.UtcDateTime;
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
    }
}