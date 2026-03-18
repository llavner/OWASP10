namespace OWASP.Domain.Models;

public class OvertimeEntry(Guid id, Guid userId, DateTime date, decimal minutes, string description, OvertimeCategory category, DateTimeOffset createdAt, DateTimeOffset updatedAt)
{
    public Guid Id { get; set; } = id;

    public Guid UserId { get; set; } = userId;

    public DateTime Date { get; set; } = date;

    private decimal _minutes = minutes;

    public decimal Minutes
    {
        get => _minutes;
        set => _minutes = value < 0 ? 0 : value;
    }

    public string Description { get; set; } = description;

    public OvertimeCategory Category { get; set; } = category;

    public DateTimeOffset CreatedAt { get; set; } = createdAt;

    public DateTimeOffset? UpdatedAt { get; set; } = updatedAt;
}

public enum OvertimeCategory
{
    Regular,
    Holiday,
    Emergency,
}
