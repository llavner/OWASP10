namespace OWASP.Application.Services;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Application.Mapping;

public class SecureOvertimeEntryService : IOvertimeService
{
    private readonly IOvertimeEntryRepository _repo;

    public SecureOvertimeEntryService(IOvertimeEntryRepository repo)
    {
        _repo = repo;
    }

    public async Task CreateEntry(OvertimeEntryCreate entry)
    {
        await _repo.AddAsync(entry.Map());
    }

    public async Task ReadEntry(OvertimeEntryResponse entry)
    {
        await _repo.GetByIdAsync(entry.Id, entry.UserId);
    }

    public async Task<IEnumerable<OvertimeEntryResponse>> ReadAllEntries(Guid userId)
    {
        var entries = await _repo.GetAllAsync(userId);
        var responses = new List<OvertimeEntryResponse>();

        foreach (var entry in entries)
        {
            entry.Map();
            responses.Add(entry.Map());
        }

        return responses;
    }

    public async Task UpdateEntry(OvertimeEntryUpdate entry)
    {
        await _repo.UpdateAsync(entry.Map());
    }

    public async Task DeleteEntry(Guid id, Guid userId)
    {
        await _repo.DeleteAsync(id, userId);
    }
}
