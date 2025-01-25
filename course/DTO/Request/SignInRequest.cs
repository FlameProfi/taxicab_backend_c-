using System.Text.Json.Serialization;

namespace course.DTO.Request;

public class SignInRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}