namespace OWASP.Domain.Models;

public sealed class Order
{
    public required int Id { get; init; }

    public required string UserId { get; init; }

    public required decimal Total { get; init; }
}
