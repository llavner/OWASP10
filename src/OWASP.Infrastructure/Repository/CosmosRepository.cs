using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

namespace OWASP.Infrastructure.Repository;

public class CosmosRepository : IOvertimeEntryRepository
{
    public Task AddAsync(OvertimeEntry entry) => throw new NotImplementedException();

    public Task DeleteAsync(long id) => throw new NotImplementedException();

    public Task<OvertimeEntry> GetByIdAsync(long id) => throw new NotImplementedException();

    public Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to) => throw new NotImplementedException();

    public Task UpdateAsync(OvertimeEntry entry) => throw new NotImplementedException();
}
