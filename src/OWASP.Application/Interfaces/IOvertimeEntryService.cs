namespace OWASP.Application.Interfaces;

using OWASP.Application.Dtos;
using OWASP.Domain.Models;

public interface IOvertimeEntryService
{
    Task<List<OvertimeEntry>> GetAllEntriesAsync(string userId);

    Task<string> AddEntryAsync(string userId, OvertimeEntryRequest request);
}
