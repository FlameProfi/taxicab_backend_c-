using System.Text.Json.Serialization;

namespace course.DTO.Response;

public class SignInResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("user")]
    public UserResponse User { get; set; }
}