using OWASP.Domain.Interfaces;

namespace OWASP.Application.Interfaces;

public interface IOvertimeEntryRepository
{
    Task UpsertRecordsAsync<T>(T record)
        where T : ICosmosEntity;

    Task<List<T>> LoadRecordsByUserIdAsync<T>(string userId);
}
