using System.Text.Json.Serialization;

namespace course.DTO.Response;

public class UserResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("fullname")]
    public string Fullname { get; set; }
}