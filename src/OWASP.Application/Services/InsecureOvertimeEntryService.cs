namespace OWASP.Application.Services;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;

public class InsecureOvertimeEntryService : IOvertimeService
{
    public Task CreateEntry(OvertimeEntryCreate entry) => throw new NotImplementedException();

    public Task DeleteEntry(Guid id, Guid userId) => throw new NotImplementedException();

    public Task<IEnumerable<OvertimeEntryResponse>> ReadAllEntries(Guid userId) => throw new NotImplementedException();

    public Task ReadEntry(OvertimeEntryResponse entry) => throw new NotImplementedException();

    public Task UpdateEntry(OvertimeEntryUpdate entry) => throw new NotImplementedException();
}
