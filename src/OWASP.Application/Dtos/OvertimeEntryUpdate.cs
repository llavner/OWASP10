using OWASP.Domain.Models;

namespace OWASP.Application.Dtos;

public class OvertimeEntryUpdate
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Minutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public OvertimeCategory Category { get; set; }
}
