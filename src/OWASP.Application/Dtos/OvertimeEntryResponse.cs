using OWASP.Domain.Models;

namespace OWASP.Application.Dtos;

public class OvertimeEntryResponse(Guid id, Guid userId, DateOnly date, decimal minutes, string description, OvertimeCategory category)
{
    public Guid Id { get; set; } = id;

    public Guid UserId { get; set; } = userId;

    public DateOnly Date { get; set; } = date;

    public decimal Minutes { get; set; } = minutes;

    public string Description { get; set; } = description;

    public OvertimeCategory Category { get; set; } = category;
}
