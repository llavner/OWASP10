namespace OWASP.Domain.Models;

public class OvertimeEntry
{
    public string Id { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Hours { get; set; }

    public int Minutes { get; set; }
}
