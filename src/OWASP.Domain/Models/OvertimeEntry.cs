using OWASP.Domain.Interfaces;

namespace OWASP.Domain.Models;

using System.Text.Json.Serialization;

public class OvertimeEntry : ICosmosEntity
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Hours { get; set; }

    public int Minutes { get; set; }
}
