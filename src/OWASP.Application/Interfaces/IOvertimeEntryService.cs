namespace OWASP.Application.Interfaces;

using OWASP.Application.Dtos;
using OWASP.Domain.Models;

public interface IOvertimeEntryService
{
    Task<List<OvertimeEntry>> GetAllEntriesAsync(string userId);

    Task AddEntryAsync(string userId, OvertimeEntryRequest request);
}
