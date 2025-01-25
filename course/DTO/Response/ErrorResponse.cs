using System.Text.Json.Serialization;

namespace course.DTO.Response;

public class ErrorResponse
{
    [JsonPropertyName("timestamp")]
    public long TimeStamp { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("error_code")]
    public string ErrorCode { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
        TimeStamp = DateTime.Now.Ticks;
        ErrorCode = Random.Shared.Next(1000, 9000).ToString();
    }
}