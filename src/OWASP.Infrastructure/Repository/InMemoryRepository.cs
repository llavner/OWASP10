namespace OWASP.Infrastructure.Repository;

using System.Collections.Concurrent;

using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class InMemoryRepository : IOvertimeEntryRepository
{
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<int, OvertimeEntry>> data = new();

    public Task AddAsync(OvertimeEntry entry) => throw new NotImplementedException();

    public Task DeleteAsync(long id) => throw new NotImplementedException();

    public Task<OvertimeEntry> GetByIdAsync(long id) => throw new NotImplementedException();

    public Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to) => throw new NotImplementedException();

    public Task UpdateAsync(OvertimeEntry entry) => throw new NotImplementedException();
}
