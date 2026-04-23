namespace OWASP.Domain.Interfaces;

public interface ICosmosEntity
{
    Guid Id { get; }

    Guid UserId { get; }
}
