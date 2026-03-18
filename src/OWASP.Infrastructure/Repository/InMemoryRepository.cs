namespace OWASP.Infrastructure.Repository;

using System.Collections.Concurrent;

using OWASP.Domain.Models;

public class InMemoryRepository
{
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<long, OvertimeEntry>> data = new();

    public Task AddAsync(OvertimeEntry entry)
    {
        var userDict = this.data.GetOrAdd(entry.UserId, _ => new ConcurrentDictionary<long, OvertimeEntry>());
        userDict[entry.Id] = entry;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid userId, long entryId)
    {
        if (this.data.TryGetValue(userId, out var userDict))
        {
            userDict.TryRemove(entryId, out _);
        }

        return Task.CompletedTask;
    }

    public Task<OvertimeEntry?> GetByIdAsync(Guid userId, long entryId)
    {
        if (this.data.TryGetValue(userId, out var userDict) && userDict.TryGetValue(entryId, out var entry))
        {
            return Task.FromResult<OvertimeEntry?>(entry);
        }

        return Task.FromResult<OvertimeEntry?>(null);
    }

    public Task UpdateAsync(OvertimeEntry entry)
    {
        if (this.data.TryGetValue(entry.UserId, out var userDict))
        {
            userDict[entry.Id] = entry;
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to)
    {
        if (this.data.TryGetValue(userId, out var userDict))
        {
            var results = userDict.Values
                .Where(entry => DateOnly.FromDateTime(entry.Date) >= from && DateOnly.FromDateTime(entry.Date) <= to)
                .ToList();
            return Task.FromResult<IReadOnlyList<OvertimeEntry>>(results);
        }

        return Task.FromResult<IReadOnlyList<OvertimeEntry>>(new List<OvertimeEntry>());
    }
}
