namespace OWASP.Application.Services;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class OvertimeEntryService(IOvertimeEntryRepository repo) : IOvertimeEntryService
{
    public async Task<string> AddEntryAsync(string userId, OvertimeEntryRequest request)
    {
        var newEntry = new OvertimeEntry()
        {
             id = Guid.NewGuid().ToString(),
             UserId = userId,
             StartDate = request.StartDate,
             EndDate = request.EndDate,
             Hours = request.Hours,
             Minutes = request.Minutes,
        };

        await repo.UpsertRecordsAsync<OvertimeEntry>(newEntry);

        return $"New entry with Id: {newEntry.id} added succesfully.";
    }

    public async Task<List<OvertimeEntry>> GetAllEntriesAsync(string userId) => await repo.LoadRecordsByUserIdAsync<OvertimeEntry>(userId);
}
