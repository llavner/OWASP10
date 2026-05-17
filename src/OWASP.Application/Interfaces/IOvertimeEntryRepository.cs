using OWASP.Domain.Interfaces;

namespace OWASP.Application.Interfaces;

public interface IOvertimeEntryRepository
{
    Task UpsertRecordsAsync<T>(T record)
        where T : ICosmosEntity;

    Task<List<T>> LoadRecordsAsync<T>();

    Task<T> LoadRecordByIdAsync<T>(string id);

    Task<T> LoadRecordByEmailAsync<T>(string email);

    Task<T> LoadRecordByTokenAsync<T>(string token);

    Task<T> LoadRecordByUserNameAsync<T>(string userName);
}
