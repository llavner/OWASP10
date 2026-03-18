using OWASP.Domain.Models;

namespace OWASP.Application.Interfaces;

public interface IOvertimeEntryRepository
{
    Task AddAsync(OvertimeEntry entry);

    Task<OvertimeEntry?> GetByIdAsync(Guid userId, Guid entryId);

    Task<IEnumerable<OvertimeEntry>> GetAllAsync(Guid userId);

    Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to);

    Task UpdateAsync(OvertimeEntry entry);

    Task DeleteAsync(Guid userId, Guid entryId);
}
