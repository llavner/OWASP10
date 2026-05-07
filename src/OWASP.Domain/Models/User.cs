using System.Text.Json.Serialization;

namespace OWASP.Domain.Models;

public class User
{
    [JsonPropertyName("id")]
    public string id { get; set; } = Guid.NewGuid().ToString();

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string? Token { get; set; } = string.Empty;

    public string? LastActive { get; set; }
}
