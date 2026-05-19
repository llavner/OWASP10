namespace OWASP.Application.Services;

using Microsoft.Extensions.Logging;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class OvertimeEntryService(IOvertimeEntryRepository repo, ILogger<OvertimeEntryService> logger, IOvertimeEntryFactory factory) : IOvertimeEntryService
{
    public async Task AddEntryAsync(string userId, OvertimeEntryRequest request)
    {
        var newEntry = factory.Create(userId, request);

        await repo.UpsertRecordsAsync<OvertimeEntry>(newEntry);

        logger.LogInformation("New entry with Id: {EntryId} added successfully for user {UserId}.", newEntry.id, userId);
    }

    public async Task<List<OvertimeEntry>> GetAllEntriesAsync(string userId)
    {
        logger.LogInformation("User: {userId} loading posts", userId);
        return await repo.LoadRecordsByUserIdAsync<OvertimeEntry>(userId);
    }
}
