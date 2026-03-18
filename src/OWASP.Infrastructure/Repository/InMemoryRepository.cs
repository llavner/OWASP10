namespace OWASP.Infrastructure.Repository;

using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class InMemoryRepository : IOvertimeEntryRepository
{
    public Task AddAsync(OvertimeEntry entry) => throw new NotImplementedException();

    public Task DeleteAsync(long id) => throw new NotImplementedException();

    public Task<OvertimeEntry> GetByIdAsync(long id) => throw new NotImplementedException();

    public Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to) => throw new NotImplementedException();

    public Task UpdateAsync(OvertimeEntry entry) => throw new NotImplementedException();
}
