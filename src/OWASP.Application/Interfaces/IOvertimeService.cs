using OWASP.Application.Dtos;

namespace OWASP.Application.Interfaces;

public interface IOvertimeService
{
    void CreateEntry(OvertimeEntryCreate entry);

    void ReadEntry(OvertimeEntryResponse entry);

    void ReadAllEntries(Guid userId);

    void UpdateEntry(OvertimeEntryUpdate entry);

    void DeleteEntry(Guid id, Guid userId);
}
