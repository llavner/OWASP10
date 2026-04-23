namespace OWASP.Infrastructure.Repository;

using System.Collections.Concurrent;

using OWASP.Domain.Models;

public sealed class InMemoryStore
{
    public InMemoryStore()
    {
        // Seed: två orders som tillhör två olika users
        Orders.TryAdd(101, new Order { Id = 101, UserId = "user-a", Total = 100 });
        Orders.TryAdd(102, new Order { Id = 102, UserId = "user-b", Total = 200 });
    }

    public ConcurrentDictionary<int, Order> Orders { get; } = new();
}
