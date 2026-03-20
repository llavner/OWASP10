using OWASP.Application.Dtos;

namespace OWASP.Application.Interfaces;

public interface IOvertimeService
{
    Task CreateEntry(OvertimeEntryCreate entry);

    Task ReadEntry(OvertimeEntryResponse entry);

    Task<IEnumerable<OvertimeEntryResponse>> ReadAllEntries(Guid userId);

    Task UpdateEntry(OvertimeEntryUpdate entry);

    Task DeleteEntry(Guid id, Guid userId);
}
