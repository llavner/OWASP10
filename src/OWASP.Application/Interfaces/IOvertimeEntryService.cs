namespace OWASP.Application.Interfaces;

using OWASP.Application.Dtos;
using OWASP.Domain.Models;

public interface IOvertimeEntryService
{
    Task<OvertimeEntry?> GetEntryByIdAsync(string userId, OvertimeEntryRequest request);

    Task<List<OvertimeEntry>> GetAllEntriesAsync(string userId);

    Task<string> AddEntryAsync(string userId, OvertimeEntryRequest request);
}
