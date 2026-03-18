using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Application.Mapping;

namespace OWASP.Application.Services;

public class SecureOvertimeEntryService : IOvertimeService
{
    private readonly IOvertimeEntryRepository _repo;

    public SecureOvertimeEntryService(IOvertimeEntryRepository repo)
    {
        _repo = repo;
    }

    public void CreateEntry(OvertimeEntryCreate entry)
    {
        _repo.AddAsync(entry.Map());
    }

    public void ReadEntry(OvertimeEntryResponse entry)
    {
    }

    public void ReadAllEntries(Guid userId)
    {
        _repo.GetAllAsync(userId);
    }

    public void DeleteEntry(Guid id, Guid userId)
    {
        _repo.DeleteAsync(id, userId);
    }

    public void UpdateEntry(OvertimeEntryUpdate entry)
    {
        _repo.UpdateAsync(entry.Map());
    }
}
