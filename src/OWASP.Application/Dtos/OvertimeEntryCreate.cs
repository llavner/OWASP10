namespace OWASP.Application.Dtos;

using OWASP.Domain.Models;

public class OvertimeEntryCreate
{
    public Guid UserId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Minutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public OvertimeCategory Category { get; set; }
}
