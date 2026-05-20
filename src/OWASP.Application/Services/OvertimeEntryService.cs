namespace OWASP.Application.Services;

using Microsoft.Extensions.Logging;

using OWASP.Application.Common;
using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

using static OWASP.Application.Common.OvertimeEntryResultCodes;

public class OvertimeEntryService(
    IOvertimeEntryRepository repo,
    ILogger<OvertimeEntryService> logger,
    IOvertimeEntryFactory factory) : IOvertimeEntryService
{
    public async Task<Result<OvertimeEntry, OvertimeEntryResultCode>> AddEntryAsync(string userId, OvertimeEntryRequest request)
    {
        var newEntry = factory.Create(userId, request);

        try
        {
            await repo.UpsertRecordsAsync<OvertimeEntry>(newEntry);
            logger.LogInformation("New entry with Id: {EntryId} added successfully for user {UserId}.", newEntry.id, userId);
            return Result<OvertimeEntry, OvertimeEntryResultCode>.Success(newEntry, OvertimeEntryResultCode.Success);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "SecurityEvent: Unauthorized access attempt by user {UserId}", userId);
            return Result<OvertimeEntry, OvertimeEntryResultCode>.Failure(OvertimeEntryResultCode.Unauthorized, "Unauthorized access.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add entry for user {UserId}", userId);
            return Result<OvertimeEntry, OvertimeEntryResultCode>.Failure(OvertimeEntryResultCode.UnknownError, "Failed to add entry.");
        }
    }

    public async Task<Result<List<OvertimeEntry>, OvertimeEntryResultCode>> GetAllEntriesAsync(string userId)
    {
        logger.LogInformation("User: {userId} loading posts", userId);

        try
        {
            var entries = await repo.LoadRecordsByUserIdAsync<OvertimeEntry>(userId);

            if (entries == null || entries.Count == 0)
            {
                return Result<List<OvertimeEntry>, OvertimeEntryResultCode>.Failure(OvertimeEntryResultCode.NotFound, "No entries found.");
            }

            return Result<List<OvertimeEntry>, OvertimeEntryResultCode>.Success(entries, OvertimeEntryResultCode.Success);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load entries for user {UserId}", userId);
            return Result<List<OvertimeEntry>, OvertimeEntryResultCode>.Failure(OvertimeEntryResultCode.UnknownError, "Failed to load entries.");
        }
    }
}
