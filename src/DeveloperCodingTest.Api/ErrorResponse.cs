using System.Text.Json;

namespace DeveloperCodingTest.Api;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
