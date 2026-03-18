using OWASP.Domain.Models;

namespace OWASP.Application.Interfaces;

public interface IOvertimeEntryRepository
{
    Task<OvertimeEntry?> GetByIdAsync(Guid userId, long id);

    Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to);

    Task AddAsync(OvertimeEntry entry);

    Task UpdateAsync(OvertimeEntry entry);

    Task DeleteAsync(Guid userId, long id);
}
